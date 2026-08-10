using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Player;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Application.LogExceptions;
using FootballAnalysis.Data.Application.Mappings;
using FootballAnalysis.Data.Application.Validations.Player;
using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Domain.Models;
using FootballAnalysis.Data.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FootballAnalysis.Data.Application.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ApplicationDbContext _context;

        public PlayerService(IPlayerRepository playerRepository, ApplicationDbContext context)
        {
            _playerRepository = playerRepository;
            _context = context;
        }

        public async Task<GetPlayerDTO> CreateAsync(CreatePlayerDTO entity)
        {
            try
            {
                var validatorResult = await new CreatePlayerValidator().ValidateAsync(entity);
                if (!validatorResult.IsValid) throw new ArgumentException("Invalid player data.");

                var player = PlayerMap.ToDomainModel(entity);
                var createdPlayer = await _playerRepository.AddAsync(player);
                return PlayerMap.ToGetDTO(createdPlayer);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while creating a player.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _playerRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while deleting a player.", ex);
            }
        }

        public async Task<IEnumerable<GetPlayerDTO>> GetAllAsync()
        {
            try
            {
                var players = await _context.Players.AsNoTracking().Include(p => p.Team).ToListAsync();
                return players.Select(PlayerMap.ToGetDTO);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching players.", ex);
            }
        }

        public async Task<GetPlayerDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var player = await _context.Players.AsNoTracking().Include(p => p.Team).FirstOrDefaultAsync(p => p.Id == id);
                return player == null ? throw new KeyNotFoundException("Player not found.") : PlayerMap.ToGetDTO(player);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching a player.", ex);
            }
        }

        public async Task<IEnumerable<GetMatchDTO>> GetMatchesAsync(Guid playerId)
        {
            await EnsurePlayerExists(playerId);
            var matches = await PlayerStats(playerId)
                .Select(pms => pms.Match)
                .Distinct()
                .OrderByDescending(m => m.KickOff)
                .ToListAsync();

            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<PlayerStatisticsDTO> GetStatisticsAsync(Guid playerId, Guid seasonId)
        {
            await EnsurePlayerExists(playerId);
            await EnsureSeasonExists(seasonId);
            var stats = await PlayerStats(playerId)
                .Where(pms => pms.Match.SeasonId == seasonId)
                .ToListAsync();

            if (!stats.Any())
            {
                throw new KeyNotFoundException("Player has no statistics for the requested season.");
            }

            return BuildStatistics(stats);
        }

        public Task<PlayerStatisticsDTO> GetSeasonStatsAsync(Guid playerId, Guid seasonId) => GetStatisticsAsync(playerId, seasonId);

        public async Task<IEnumerable<GetMatchDTO>> GetLast5MatchesAsync(Guid playerId)
        {
            await EnsurePlayerExists(playerId);
            var matches = await PlayerStats(playerId)
                .Select(pms => pms.Match)
                .Distinct()
                .OrderByDescending(m => m.KickOff)
                .Take(5)
                .ToListAsync();

            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<double>> GetRatingsAsync(Guid playerId)
        {
            await EnsurePlayerExists(playerId);
            return await _context.PlayerMatchStats
                .AsNoTracking()
                .Where(pms => pms.PlayerId == playerId)
                .OrderByDescending(pms => pms.Match.KickOff)
                .Select(pms => pms.PlayerStats.FotmobRating)
                .ToListAsync();
        }

        public async Task<int> GetGoalsAsync(Guid playerId)
        {
            await EnsurePlayerExists(playerId);
            return await _context.PlayerMatchStats.AsNoTracking().Where(pms => pms.PlayerId == playerId).SumAsync(pms => pms.PlayerAttack.Goals);
        }

        public async Task<int> GetAssistsAsync(Guid playerId)
        {
            await EnsurePlayerExists(playerId);
            return await _context.PlayerMatchStats.AsNoTracking().Where(pms => pms.PlayerId == playerId).SumAsync(pms => pms.PlayerPasses.Assists);
        }

        public async Task<PlayerComparisonDTO> CompareAsync(Guid playerId, Guid otherPlayerId)
        {
            await EnsurePlayerExists(playerId);
            await EnsurePlayerExists(otherPlayerId);
            var player = BuildStatistics(await PlayerStats(playerId).ToListAsync());
            var otherPlayer = BuildStatistics(await PlayerStats(otherPlayerId).ToListAsync());
            return new PlayerComparisonDTO(player, otherPlayer);
        }

        public async Task<GetPlayerDTO> UpdateAsync(Guid id, UpdatePlayerDTO entity)
        {
            try
            {
                var validatorResult = await new UpdatePlayerValidator().ValidateAsync(entity);
                if (!validatorResult.IsValid) throw new ArgumentException("Invalid player data.");

                var existingPlayer = await _playerRepository.GetAsync(id);
                existingPlayer.FirstName = entity.FirstName ?? existingPlayer.FirstName;
                existingPlayer.LastName = entity.LastName ?? existingPlayer.LastName;
                existingPlayer.Position = entity.Position ?? existingPlayer.Position;
                existingPlayer.AlternativePositions = entity.AlternativePositions ?? existingPlayer.AlternativePositions;
                existingPlayer.ShirtNumber = entity.ShirtNumber != 0 ? entity.ShirtNumber : existingPlayer.ShirtNumber;
                existingPlayer.IsCaptain = entity.IsCaptain;
                existingPlayer.IsActive = entity.IsActive;

                if (entity.TeamId.HasValue && entity.TeamId.Value != Guid.Empty && existingPlayer.TeamId != entity.TeamId.Value)
                {
                    existingPlayer.TeamId = entity.TeamId.Value;
                }

                var updatedPlayer = await _playerRepository.UpdateAsync(existingPlayer);
                return PlayerMap.ToGetDTO(updatedPlayer);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while updating a player.", ex);
            }
        }

        private IQueryable<PlayerMatchStats> PlayerStats(Guid playerId)
        {
            return _context.PlayerMatchStats
                .AsNoTracking()
                .Include(pms => pms.Player)
                .ThenInclude(p => p.Team)
                .Include(pms => pms.Match)
                .ThenInclude(m => m.HomeTeam)
                .Include(pms => pms.Match)
                .ThenInclude(m => m.AwayTeam)
                .Include(pms => pms.Match)
                .ThenInclude(m => m.Competition)
                .Include(pms => pms.Match)
                .ThenInclude(m => m.Season)
                .Where(pms => pms.PlayerId == playerId);
        }

        private async Task EnsurePlayerExists(Guid playerId)
        {
            if (!await _context.Players.AsNoTracking().AnyAsync(p => p.Id == playerId))
            {
                throw new KeyNotFoundException("Player not found.");
            }
        }

        private async Task EnsureSeasonExists(Guid seasonId)
        {
            if (!await _context.Seasons.AsNoTracking().AnyAsync(s => s.Id == seasonId))
            {
                throw new KeyNotFoundException("Season not found.");
            }
        }

        private static PlayerStatisticsDTO BuildStatistics(IEnumerable<PlayerMatchStats> stats)
        {
            var statList = stats.ToList();
            var player = statList.FirstOrDefault()?.Player ?? throw new KeyNotFoundException("Player statistics not found.");
            var shots = statList.Sum(pms => pms.PlayerAttack.TotalShots);
            var shotsOnTarget = statList.Sum(pms => pms.PlayerAttack.ShotsOnTarget);
            var passesAttempted = statList.Sum(pms => pms.PlayerPasses.PassesAttempted);
            var accuratePasses = statList.Sum(pms => pms.PlayerPasses.AccuratePasses);

            return new PlayerStatisticsDTO(
                player.Id,
                player.FirstName,
                player.LastName,
                statList.Count,
                statList.Count(pms => pms.Started),
                statList.Sum(pms => pms.MinutesPlayed),
                statList.Sum(pms => pms.PlayerAttack.Goals),
                statList.Sum(pms => pms.PlayerPasses.Assists),
                Average(statList, pms => pms.PlayerStats.FotmobRating),
                Average(statList, pms => pms.PlayerStats.SofascoreRating),
                shots,
                shotsOnTarget,
                Percentage(shotsOnTarget, shots),
                accuratePasses,
                passesAttempted,
                Percentage(accuratePasses, passesAttempted),
                statList.Sum(pms => pms.PlayerPasses.ChancesCreated),
                statList.Sum(pms => pms.PlayerDefence.Tackles),
                statList.Sum(pms => pms.PlayerDefence.Interceptions),
                statList.Sum(pms => pms.PlayerDefence.Recoveries),
                statList.Sum(pms => pms.Goalkeepering.Saves),
                statList.Sum(pms => pms.PlayerDiscipline.YellowCards),
                statList.Sum(pms => pms.PlayerDiscipline.RedCards));
        }

        private static double Percentage(int numerator, int denominator) => denominator == 0 ? 0 : (double)numerator / denominator * 100;

        private static double Average<T>(IEnumerable<T> source, Func<T, double> selector) => source.Any() ? source.Average(selector) : 0;
    }
}
