using FootballAnalysis.Data.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballAnalysis.Data.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamMatchStats> TeamMatchStats { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerMatchStats> PlayerMatchStats { get; set; }

    }
}
