using Serilog;

namespace FootballAnalysis.Data.Application.LogExceptions
{
    public static class LogException
    {
        public static void LogExceptions(Exception ex)
        {
            // Log the full exception (including stack) at Error level.
            // Avoid logging only the message because it loses stack trace and inner exceptions.
            Log.Error(ex, "Unhandled exception");
        }

        // String-based helpers (preserve for backward compatibility)
        public static void LogToFile(string message) => Log.Information(message);
        public static void LogToConsole(string message) => Log.Warning(message);
        public static void LogToDebugger(string message) => Log.Debug(message);

        // Exception-aware helpers that preserve full exception details
        public static void LogToFile(Exception ex) => Log.Error(ex, ex?.Message ?? "Exception");
        public static void LogToConsole(Exception ex) => Log.Warning(ex, ex?.Message ?? "Exception");
        public static void LogToDebugger(Exception ex) => Log.Debug(ex, ex?.Message ?? "Exception");
    }
}
