using FootballAnalysis.Data.Application.DTOs.Common;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface ILeaderboardService
    {
        Task<IEnumerable<PlayerLeaderDTO>> GetGoalsAsync();
        Task<IEnumerable<PlayerLeaderDTO>> GetAssistsAsync();
        Task<IEnumerable<PlayerLeaderDTO>> GetRatingsAsync();
        Task<IEnumerable<PlayerLeaderDTO>> GetPassesAsync();
        Task<IEnumerable<PlayerLeaderDTO>> GetChancesCreatedAsync();
        Task<IEnumerable<PlayerLeaderDTO>> GetTacklesAsync();
        Task<IEnumerable<PlayerLeaderDTO>> GetInterceptionsAsync();
        Task<IEnumerable<PlayerLeaderDTO>> GetCleanSheetsAsync();
        Task<IEnumerable<PlayerLeaderDTO>> GetSavesAsync();
    }
}
