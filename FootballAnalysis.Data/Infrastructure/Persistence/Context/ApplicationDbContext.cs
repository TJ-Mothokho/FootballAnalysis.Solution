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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Team>()
                .HasOne(team => team.Captain)
                .WithMany()
                .HasForeignKey(team => team.CaptainId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany(team => team.Players)
                .WithOne(player => player.Team)
                .HasForeignKey(player => player.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(match => match.HomeTeam)
                .WithMany()
                .HasForeignKey(match => match.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(match => match.AwayTeam)
                .WithMany()
                .HasForeignKey(match => match.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<TeamMatchStats>().OwnsOne(stats => stats.MatchStats);
            modelBuilder.Entity<TeamMatchStats>().OwnsOne(stats => stats.MatchShots);
            modelBuilder.Entity<TeamMatchStats>().OwnsOne(stats => stats.MatchExpectedGoals);
            modelBuilder.Entity<TeamMatchStats>().OwnsOne(stats => stats.MatchPasses);
            modelBuilder.Entity<TeamMatchStats>().OwnsOne(stats => stats.MatchDiscipline);
            modelBuilder.Entity<TeamMatchStats>().OwnsOne(stats => stats.MatchDefence);
            modelBuilder.Entity<TeamMatchStats>().OwnsOne(stats => stats.MatchDuels);
            modelBuilder.Entity<TeamMatchStats>().OwnsOne(stats => stats.MatchAttackingZones);
            modelBuilder.Entity<TeamMatchStats>().OwnsOne(stats => stats.MatchAnalysis);

            modelBuilder.Entity<PlayerMatchStats>().OwnsOne(stats => stats.Analysis);
            modelBuilder.Entity<PlayerMatchStats>().OwnsOne(stats => stats.PlayerStats);
            modelBuilder.Entity<PlayerMatchStats>().OwnsOne(stats => stats.PlayerAttack);
            modelBuilder.Entity<PlayerMatchStats>().OwnsOne(stats => stats.PlayerPasses);
            modelBuilder.Entity<PlayerMatchStats>().OwnsOne(stats => stats.PlayerDefence);
            modelBuilder.Entity<PlayerMatchStats>().OwnsOne(stats => stats.PlayerDuels);
            modelBuilder.Entity<PlayerMatchStats>().OwnsOne(stats => stats.Goalkeepering);
            modelBuilder.Entity<PlayerMatchStats>().OwnsOne(stats => stats.PlayerDiscipline);
        }
    }
}
