using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Player;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface IPlayerService : IService<GetPlayerDTO, CreatePlayerDTO, UpdatePlayerDTO>
    {
        Task<IEnumerable<GetMatchDTO>> GetMatchesAsync(Guid playerId);
        Task<PlayerStatisticsDTO> GetStatisticsAsync(Guid playerId, Guid seasonId);
        Task<PlayerStatisticsDTO> GetSeasonStatsAsync(Guid playerId, Guid seasonId);
        Task<IEnumerable<GetMatchDTO>> GetLast5MatchesAsync(Guid playerId);
        Task<IEnumerable<double>> GetRatingsAsync(Guid playerId);
        Task<int> GetGoalsAsync(Guid playerId);
        Task<int> GetAssistsAsync(Guid playerId);
        Task<PlayerComparisonDTO> CompareAsync(Guid playerId, Guid otherPlayerId);
    }
}
