using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Application.Mappings;
using FootballAnalysis.Data.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FootballAnalysis.Data.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILeaderboardService _leaderboardService;

        public DashboardService(ApplicationDbContext context, ILeaderboardService leaderboardService)
        {
            _context = context;
            _leaderboardService = leaderboardService;
        }

        public async Task<DashboardOverviewDTO> GetOverviewAsync()
        {
            var matches = await _context.Matches.AsNoTracking().ToListAsync();
            var completed = matches.Count(m => m.KickOff <= DateTime.Now);
            return new DashboardOverviewDTO(
                await _context.Competitions.AsNoTracking().CountAsync(),
                await _context.Seasons.AsNoTracking().CountAsync(),
                await _context.Teams.AsNoTracking().CountAsync(),
                await _context.Players.AsNoTracking().CountAsync(),
                matches.Count,
                completed,
                matches.Count - completed,
                matches.Where(m => m.KickOff <= DateTime.Now).Sum(m => m.HomeGoals + m.AwayGoals));
        }

        public Task<IEnumerable<PlayerLeaderDTO>> GetTopScorersAsync() => _leaderboardService.GetGoalsAsync();

        public Task<IEnumerable<PlayerLeaderDTO>> GetTopAssistsAsync() => _leaderboardService.GetAssistsAsync();

        public Task<IEnumerable<PlayerLeaderDTO>> GetTopRatedAsync() => _leaderboardService.GetRatingsAsync();

        public async Task<IEnumerable<StandingDTO>> GetFormTableAsync()
        {
            var stats = await _context.TeamMatchStats
                .AsNoTracking()
                .Include(tms => tms.Team)
                .Include(tms => tms.Match)
                .Where(tms => tms.Match.KickOff <= DateTime.Now)
                .OrderByDescending(tms => tms.Match.KickOff)
                .ToListAsync();

            return stats
                .GroupBy(tms => tms.Team)
                .Select(group =>
                {
                    var lastFive = group.OrderByDescending(tms => tms.Match.KickOff).Take(5).ToList();
                    var wins = lastFive.Count(tms => tms.MatchStats.TeamGoals > tms.MatchStats.OppositionGoals);
                    var draws = lastFive.Count(tms => tms.MatchStats.TeamGoals == tms.MatchStats.OppositionGoals);
                    var losses = lastFive.Count(tms => tms.MatchStats.TeamGoals < tms.MatchStats.OppositionGoals);
                    var goalsFor = lastFive.Sum(tms => tms.MatchStats.TeamGoals);
                    var goalsAgainst = lastFive.Sum(tms => tms.MatchStats.OppositionGoals);
                    return new StandingDTO(0, group.Key.Id, group.Key.Name, lastFive.Count, wins, draws, losses, goalsFor, goalsAgainst, goalsFor - goalsAgainst, wins * 3 + draws);
                })
                .OrderByDescending(row => row.Points)
                .ThenByDescending(row => row.GoalDifference)
                .Select((row, index) => row with { Position = index + 1 });
        }

        public async Task<IEnumerable<GetMatchDTO>> GetRecentMatchesAsync()
        {
            var matches = await MatchGraph().Where(m => m.KickOff <= DateTime.Now).OrderByDescending(m => m.KickOff).Take(10).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetMatchDTO>> GetUpcomingFixturesAsync()
        {
            var matches = await MatchGraph().Where(m => m.KickOff > DateTime.Now).OrderBy(m => m.KickOff).Take(10).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<GoalsPerRoundDTO>> GetGoalsPerRoundAsync()
        {
            var matches = await _context.Matches.AsNoTracking().Where(m => m.KickOff <= DateTime.Now).ToListAsync();
            return matches
                .GroupBy(m => DateOnly.FromDateTime(m.KickOff.Date))
                .Select(group => new GoalsPerRoundDTO(group.Key, group.Sum(m => m.HomeGoals + m.AwayGoals)))
                .OrderBy(row => row.Date);
        }

        private IQueryable<FootballAnalysis.Data.Domain.Models.Match> MatchGraph()
        {
            return _context.Matches
                .AsNoTracking()
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.Competition)
                .Include(m => m.Season);
        }
    }
}
