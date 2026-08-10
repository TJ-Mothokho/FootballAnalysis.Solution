using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Player;
using FootballAnalysis.Data.Application.DTOs.Season;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Application.LogExceptions;
using FootballAnalysis.Data.Application.Mappings;
using FootballAnalysis.Data.Application.Validations.Season;
using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Domain.Models;
using FootballAnalysis.Data.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FootballAnalysis.Data.Application.Services
{
    public class SeasonService : ISeasonService
    {
        private readonly ISeasonRepository _seasonRepository;
        private readonly ApplicationDbContext _context;

        public SeasonService(ISeasonRepository seasonRepository, ApplicationDbContext context)
        {
            _seasonRepository = seasonRepository;
            _context = context;
        }

        public async Task<GetSeasonDTO> CreateAsync(CreateSeasonDTO entity)
        {
            try
            {
                var validatorResult = await new CreateSeasonValidator().ValidateAsync(entity);
                if (!validatorResult.IsValid) throw new ArgumentException("Invalid season data.");

                var season = SeasonMap.ToDomainModel(entity);
                var createdSeason = await _seasonRepository.AddAsync(season);
                return SeasonMap.ToGetDTO(createdSeason);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while creating a season.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _seasonRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while deleting a season.", ex);
            }
        }

        public async Task<IEnumerable<GetSeasonDTO>> GetAllAsync()
        {
            try
            {
                var seasons = await _context.Seasons.AsNoTracking().ToListAsync();
                return seasons.Select(SeasonMap.ToGetDTO);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching seasons.", ex);
            }
        }

        public async Task<GetSeasonDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var season = await _context.Seasons.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
                return season == null ? throw new KeyNotFoundException("Season not found.") : SeasonMap.ToGetDTO(season);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching a season.", ex);
            }
        }

        public async Task<GetSeasonStatisticsDTO> GetSeasonStatisticsAsync(Guid id)
        {
            var season = await _context.Seasons.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (season == null) throw new KeyNotFoundException("Season not found.");

            var matches = await _context.Matches.AsNoTracking().Where(m => m.SeasonId == id).ToListAsync();
            var teamIds = matches.SelectMany(m => new[] { m.HomeTeamId, m.AwayTeamId }).Distinct().ToList();
            var playerCount = await _context.Players.AsNoTracking().CountAsync(p => teamIds.Contains(p.TeamId));
            var goals = matches.Sum(m => m.HomeGoals + m.AwayGoals);
            var completed = matches.Count(m => m.KickOff <= DateTime.Now);

            return new GetSeasonStatisticsDTO
            {
                SeasonId = season.Id,
                SeasonName = season.Name,
                Matches = matches.Count,
                CompletedMatches = completed,
                UpcomingMatches = matches.Count - completed,
                Teams = teamIds.Count,
                Players = playerCount,
                Goals = goals,
                AverageGoalsPerMatch = completed == 0 ? 0 : (double)goals / completed
            };
        }

        public async Task<IEnumerable<GetMatchDTO>> GetSeasonMatchesAsync(Guid id)
        {
            await EnsureSeasonExists(id);
            var matches = await MatchGraph().Where(m => m.SeasonId == id).OrderByDescending(m => m.KickOff).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetPlayerDTO>> GetSeasonPlayersAsync(Guid id)
        {
            await EnsureSeasonExists(id);
            var teamIds = await _context.Matches.AsNoTracking()
                .Where(m => m.SeasonId == id)
                .SelectMany(m => new[] { m.HomeTeamId, m.AwayTeamId })
                .Distinct()
                .ToListAsync();

            var players = await _context.Players.AsNoTracking().Include(p => p.Team).Where(p => teamIds.Contains(p.TeamId)).ToListAsync();
            return players.Select(PlayerMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetTeamDTO>> GetSeasonTeamsAsync(Guid id)
        {
            await EnsureSeasonExists(id);
            var teamIds = await _context.Matches.AsNoTracking()
                .Where(m => m.SeasonId == id)
                .SelectMany(m => new[] { m.HomeTeamId, m.AwayTeamId })
                .Distinct()
                .ToListAsync();

            var teams = await _context.Teams.AsNoTracking().Where(t => teamIds.Contains(t.Id)).ToListAsync();
            return teams.Select(TeamMap.ToGetDTO);
        }

        public async Task<IEnumerable<PlayerLeaderDTO>> GetSeasonLeadersAsync(Guid id)
        {
            await EnsureSeasonExists(id);
            var stats = await _context.PlayerMatchStats
                .AsNoTracking()
                .Include(pms => pms.Player)
                .ThenInclude(p => p.Team)
                .Include(pms => pms.Match)
                .Where(pms => pms.Match.SeasonId == id)
                .ToListAsync();

            return stats
                .GroupBy(pms => pms.Player)
                .Select(group => new PlayerLeaderDTO(
                    group.Key.Id,
                    group.Key.FirstName,
                    group.Key.LastName,
                    group.Key.Team?.Name ?? "",
                    group.Sum(pms => pms.PlayerAttack.Goals),
                    group.Any() ? group.Average(pms => pms.PlayerStats.FotmobRating) : 0))
                .OrderByDescending(player => player.Total)
                .Take(10);
        }

        public async Task<GetSeasonDTO> UpdateAsync(Guid id, UpdateSeasonDTO entity)
        {
            try
            {
                var validatorResult = await new UpdateSeasonValidator().ValidateAsync(entity);
                if (!validatorResult.IsValid) throw new ArgumentException("Invalid season data.");

                var existingSeason = await _seasonRepository.GetAsync(id);
                existingSeason.StartDate = entity.StartDate;
                existingSeason.EndDate = entity.EndDate;
                existingSeason.IsCurrent = entity.IsCurrent;

                var updatedSeason = await _seasonRepository.UpdateAsync(existingSeason);
                return SeasonMap.ToGetDTO(updatedSeason);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while updating a season.", ex);
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

        private async Task EnsureSeasonExists(Guid id)
        {
            if (!await _context.Seasons.AsNoTracking().AnyAsync(s => s.Id == id))
            {
                throw new KeyNotFoundException("Season not found.");
            }
        }
    }
}
