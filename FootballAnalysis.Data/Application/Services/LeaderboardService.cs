using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Domain.Models;
using FootballAnalysis.Data.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FootballAnalysis.Data.Application.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        private readonly ApplicationDbContext _context;

        public LeaderboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PlayerLeaderDTO>> GetGoalsAsync() => await Leaders(pms => pms.PlayerAttack.Goals);

        public async Task<IEnumerable<PlayerLeaderDTO>> GetAssistsAsync() => await Leaders(pms => pms.PlayerPasses.Assists);

        public async Task<IEnumerable<PlayerLeaderDTO>> GetRatingsAsync()
        {
            var stats = await PlayerStats().ToListAsync();
            return stats.GroupBy(pms => pms.Player)
                .Select(group => ToLeader(group, group.Count()))
                .OrderByDescending(player => player.AverageRating)
                .Take(20);
        }

        public async Task<IEnumerable<PlayerLeaderDTO>> GetPassesAsync() => await Leaders(pms => pms.PlayerPasses.AccuratePasses);

        public async Task<IEnumerable<PlayerLeaderDTO>> GetChancesCreatedAsync() => await Leaders(pms => pms.PlayerPasses.ChancesCreated);

        public async Task<IEnumerable<PlayerLeaderDTO>> GetTacklesAsync() => await Leaders(pms => pms.PlayerDefence.Tackles);

        public async Task<IEnumerable<PlayerLeaderDTO>> GetInterceptionsAsync() => await Leaders(pms => pms.PlayerDefence.Interceptions);

        public async Task<IEnumerable<PlayerLeaderDTO>> GetCleanSheetsAsync() => await Leaders(pms => pms.Goalkeepering.GoalsConceded == 0 && pms.MinutesPlayed > 0 ? 1 : 0);

        public async Task<IEnumerable<PlayerLeaderDTO>> GetSavesAsync() => await Leaders(pms => pms.Goalkeepering.Saves);

        private async Task<IEnumerable<PlayerLeaderDTO>> Leaders(Func<PlayerMatchStats, int> selector)
        {
            var stats = await PlayerStats().ToListAsync();
            return stats.GroupBy(pms => pms.Player)
                .Select(group => ToLeader(group, group.Sum(selector)))
                .OrderByDescending(player => player.Total)
                .ThenByDescending(player => player.AverageRating)
                .Take(20);
        }

        private IQueryable<PlayerMatchStats> PlayerStats()
        {
            return _context.PlayerMatchStats
                .AsNoTracking()
                .Include(pms => pms.Player)
                .ThenInclude(p => p.Team);
        }

        private static PlayerLeaderDTO ToLeader(IGrouping<Player, PlayerMatchStats> group, int total)
        {
            return new PlayerLeaderDTO(
                group.Key.Id,
                group.Key.FirstName,
                group.Key.LastName,
                group.Key.Team?.Name ?? "",
                total,
                group.Any() ? group.Average(pms => pms.PlayerStats.FotmobRating) : 0);
        }
    }
}
