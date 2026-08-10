using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.PlayerMatchStats;
using FootballAnalysis.Data.Application.DTOs.TeamMatchStats;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Application.LogExceptions;
using FootballAnalysis.Data.Application.Mappings;
using FootballAnalysis.Data.Application.Validations.Match;
using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Domain.Models;
using FootballAnalysis.Data.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FootballAnalysis.Data.Application.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly ApplicationDbContext _context;

        public MatchService(IMatchRepository matchRepository, ApplicationDbContext context)
        {
            _matchRepository = matchRepository;
            _context = context;
        }

        public async Task<GetMatchDTO> CreateAsync(CreateMatchDTO entity)
        {
            try
            {
                var validatorResult = await new CreateMatchValidator().ValidateAsync(entity);
                if (!validatorResult.IsValid) throw new ArgumentException("Invalid match data.");

                var match = MatchMap.ToDomainModel(entity);
                var createdMatch = await _matchRepository.AddAsync(match);
                return MatchMap.ToGetDTO(createdMatch);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while creating a match.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _matchRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while deleting a match.", ex);
            }
        }

        public async Task<IEnumerable<GetMatchDTO>> GetAllAsync()
        {
            try
            {
                var matches = await MatchGraph().OrderByDescending(m => m.KickOff).ToListAsync();
                return matches.Select(MatchMap.ToGetDTO);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching matches.", ex);
            }
        }

        public async Task<GetMatchDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var match = await MatchGraph().FirstOrDefaultAsync(m => m.Id == id);
                return match == null ? throw new KeyNotFoundException("Match not found.") : MatchMap.ToGetDTO(match);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching a match.", ex);
            }
        }

        public async Task<IEnumerable<GetMatchDTO>> GetUpcomingAsync()
        {
            var matches = await MatchGraph().Where(m => m.KickOff > DateTime.Now).OrderBy(m => m.KickOff).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetMatchDTO>> GetCompletedAsync()
        {
            var matches = await MatchGraph().Where(m => m.KickOff <= DateTime.Now).OrderByDescending(m => m.KickOff).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetMatchDTO>> GetLatestAsync()
        {
            var matches = await MatchGraph().Where(m => m.KickOff <= DateTime.Now).OrderByDescending(m => m.KickOff).Take(10).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetMatchDTO>> GetTodayAsync()
        {
            var today = DateTime.Today;
            var tomorrow = today.AddDays(1);
            var matches = await MatchGraph().Where(m => m.KickOff >= today && m.KickOff < tomorrow).OrderBy(m => m.KickOff).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetMatchDTO>> GetByTeamAsync(Guid teamId)
        {
            var matches = await MatchGraph().Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId).OrderByDescending(m => m.KickOff).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetMatchDTO>> GetByCompetitionAsync(Guid competitionId)
        {
            var matches = await MatchGraph().Where(m => m.CompetitionId == competitionId).OrderByDescending(m => m.KickOff).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetMatchDTO>> GetBySeasonAsync(Guid seasonId)
        {
            var matches = await MatchGraph().Where(m => m.SeasonId == seasonId).OrderByDescending(m => m.KickOff).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetTeamMatchStatsDTO>> GetTeamStatsAsync(Guid matchId)
        {
            await EnsureMatchExists(matchId);
            var stats = await TeamStatsGraph().Where(tms => tms.MatchId == matchId).ToListAsync();
            return stats.Select(TeamMatchStatsMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetPlayerMatchStatsDTO>> GetPlayerStatsAsync(Guid matchId)
        {
            await EnsureMatchExists(matchId);
            var stats = await PlayerStatsGraph().Where(pms => pms.MatchId == matchId).ToListAsync();
            return stats.Select(PlayerMatchStatsMap.ToGetDTO);
        }

        public async Task<MatchSummaryDTO> GetSummaryAsync(Guid matchId)
        {
            var match = await MatchGraph().FirstOrDefaultAsync(m => m.Id == matchId);
            if (match == null) throw new KeyNotFoundException("Match not found.");

            var teamStats = await TeamStatsGraph().Where(tms => tms.MatchId == matchId).ToListAsync();
            var playerStats = await PlayerStatsGraph().Where(pms => pms.MatchId == matchId).ToListAsync();
            var homeStats = teamStats.FirstOrDefault(tms => tms.TeamId == match.HomeTeamId);
            var awayStats = teamStats.FirstOrDefault(tms => tms.TeamId == match.AwayTeamId);

            return new MatchSummaryDTO(
                MatchMap.ToGetDTO(match),
                $"{match.HomeTeam.Name} {match.HomeGoals}-{match.AwayGoals} {match.AwayTeam.Name}",
                homeStats == null ? null : TeamMatchStatsMap.ToGetDTO(homeStats),
                awayStats == null ? null : TeamMatchStatsMap.ToGetDTO(awayStats),
                playerStats.OrderByDescending(pms => pms.PlayerStats.FotmobRating).Take(5).Select(PlayerMatchStatsMap.ToGetDTO),
                playerStats.Where(pms => pms.PlayerAttack.Goals > 0).OrderByDescending(pms => pms.PlayerAttack.Goals).Select(PlayerMatchStatsMap.ToGetDTO),
                playerStats.Where(pms => pms.PlayerPasses.Assists > 0).OrderByDescending(pms => pms.PlayerPasses.Assists).Select(PlayerMatchStatsMap.ToGetDTO));
        }

        public async Task<MatchWorkspaceDTO> GetWorkspaceAsync(Guid matchId)
        {
            var match = await MatchGraph().FirstOrDefaultAsync(m => m.Id == matchId);
            if (match == null) throw new KeyNotFoundException("Match not found.");

            var teamStats = await TeamStatsGraph().Where(tms => tms.MatchId == matchId).ToListAsync();
            var playerStats = await PlayerStatsGraph().Where(pms => pms.MatchId == matchId).ToListAsync();
            var homeStats = teamStats.FirstOrDefault(tms => tms.TeamId == match.HomeTeamId);
            var awayStats = teamStats.FirstOrDefault(tms => tms.TeamId == match.AwayTeamId);

            return new MatchWorkspaceDTO(
                MatchMap.ToGetDTO(match),
                TeamMap.ToGetDTO(match.HomeTeam),
                TeamMap.ToGetDTO(match.AwayTeam),
                CompetitionMap.ToGetDTO(match.Competition),
                SeasonMap.ToGetDTO(match.Season),
                homeStats == null ? null : TeamMatchStatsMap.ToGetDTO(homeStats),
                awayStats == null ? null : TeamMatchStatsMap.ToGetDTO(awayStats),
                playerStats.Where(pms => pms.TeamId == match.HomeTeamId).Select(PlayerMatchStatsMap.ToGetDTO),
                playerStats.Where(pms => pms.TeamId == match.AwayTeamId).Select(PlayerMatchStatsMap.ToGetDTO));
        }

        public async Task<GetMatchDTO> UpdateAsync(Guid id, UpdateMatchDTO entity)
        {
            try
            {
                var validatorResult = await new UpdateMatchValidator().ValidateAsync(entity);
                if (!validatorResult.IsValid) throw new ArgumentException("Invalid match data.");

                var existingMatch = await _matchRepository.GetAsync(id);
                existingMatch.KickOff = entity.KickOff;
                existingMatch.Venue = entity.Venue ?? existingMatch.Venue;
                existingMatch.Status = entity.Status ?? existingMatch.Status;
                existingMatch.Attendance = entity.Attendance ?? existingMatch.Attendance;
                existingMatch.Referee = entity.Referee ?? existingMatch.Referee;
                existingMatch.HomeGoals = entity.HomeGoals;
                existingMatch.AwayGoals = entity.AwayGoals;

                var updatedMatch = await _matchRepository.UpdateAsync(existingMatch);
                return MatchMap.ToGetDTO(updatedMatch);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while updating a match.", ex);
            }
        }

        private IQueryable<Match> MatchGraph()
        {
            return _context.Matches
                .AsNoTracking()
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.Competition)
                .Include(m => m.Season);
        }

        private IQueryable<TeamMatchStats> TeamStatsGraph()
        {
            return _context.TeamMatchStats
                .AsNoTracking()
                .Include(tms => tms.Match)
                .ThenInclude(m => m.HomeTeam)
                .Include(tms => tms.Match)
                .ThenInclude(m => m.AwayTeam)
                .Include(tms => tms.Match)
                .ThenInclude(m => m.Competition)
                .Include(tms => tms.Match)
                .ThenInclude(m => m.Season)
                .Include(tms => tms.Team);
        }

        private IQueryable<PlayerMatchStats> PlayerStatsGraph()
        {
            return _context.PlayerMatchStats
                .AsNoTracking()
                .Include(pms => pms.Player)
                .ThenInclude(p => p.Team)
                .Include(pms => pms.Team)
                .Include(pms => pms.Match)
                .ThenInclude(m => m.HomeTeam)
                .Include(pms => pms.Match)
                .ThenInclude(m => m.AwayTeam)
                .Include(pms => pms.Match)
                .ThenInclude(m => m.Competition)
                .Include(pms => pms.Match)
                .ThenInclude(m => m.Season);
        }

        private async Task EnsureMatchExists(Guid matchId)
        {
            if (!await _context.Matches.AsNoTracking().AnyAsync(m => m.Id == matchId))
            {
                throw new KeyNotFoundException("Match not found.");
            }
        }
    }
}
