using FluentValidation;
using FootballAnalysis.Data.Application.DTOs.Competition;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Application.Services;
using FootballAnalysis.Data.Application.Validations.Competition;
using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Infrastructure.Persistence.Context;
using FootballAnalysis.Data.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application
{
    public static class Extensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            // Services
            services.AddScoped<ICompetitionService, CompetitionService>();

            // Validation
            services.AddScoped<IValidator<CreateCompetitionDTO>, CreateCompetitionValidator>();
            services.AddScoped<IValidator<UpdateCompetitionDTO>, UpdateCompetitionValidator>();

            var fileName = configuration.GetValue<string>("SerilogFile:FileName") ?? "FootballAnalysisApp";

            // Configure Serilog logging
            // - Use a more suitable file extension (.log)
            // - Enrich logs with context, machine and thread id
            // - Use DEBUG minimum level in development builds
            // - Add sensible file size and retention limits
            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
                .Enrich.FromLogContext()
                // Add machine name as a static property (no extra package required)
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .Enrich.WithProperty("Application", fileName)
                // Debug sink for development diagnostics (only emits when MinimumLevel allows)
                .WriteTo.Debug(restrictedToMinimumLevel: LogEventLevel.Debug)
                // Console sink for realtime monitoring
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                // Rolling file sink with size limits and retention
                .WriteTo.File(path: $"{fileName}-.log",
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: true,
                    fileSizeLimitBytes: 10 * 1024 * 1024, // 10 MB per file
                    retainedFileCountLimit: 31)
                .CreateLogger();

            return services;
        }
    }
}
