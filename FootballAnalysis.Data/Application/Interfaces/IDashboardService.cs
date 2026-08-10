using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardOverviewDTO> GetOverviewAsync();
        Task<IEnumerable<PlayerLeaderDTO>> GetTopScorersAsync();
        Task<IEnumerable<PlayerLeaderDTO>> GetTopAssistsAsync();
        Task<IEnumerable<PlayerLeaderDTO>> GetTopRatedAsync();
        Task<IEnumerable<StandingDTO>> GetFormTableAsync();
        Task<IEnumerable<GetMatchDTO>> GetRecentMatchesAsync();
        Task<IEnumerable<GetMatchDTO>> GetUpcomingFixturesAsync();
        Task<IEnumerable<GoalsPerRoundDTO>> GetGoalsPerRoundAsync();
    }
}
