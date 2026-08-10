using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.PlayerMatchStats;
using FootballAnalysis.Data.Application.DTOs.TeamMatchStats;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface IMatchService : IService<GetMatchDTO, CreateMatchDTO, UpdateMatchDTO>
    {
        Task<IEnumerable<GetMatchDTO>> GetUpcomingAsync();
        Task<IEnumerable<GetMatchDTO>> GetCompletedAsync();
        Task<IEnumerable<GetMatchDTO>> GetLatestAsync();
        Task<IEnumerable<GetMatchDTO>> GetTodayAsync();
        Task<IEnumerable<GetMatchDTO>> GetByTeamAsync(Guid teamId);
        Task<IEnumerable<GetMatchDTO>> GetByCompetitionAsync(Guid competitionId);
        Task<IEnumerable<GetMatchDTO>> GetBySeasonAsync(Guid seasonId);
        Task<IEnumerable<GetTeamMatchStatsDTO>> GetTeamStatsAsync(Guid matchId);
        Task<IEnumerable<GetPlayerMatchStatsDTO>> GetPlayerStatsAsync(Guid matchId);
        Task<MatchSummaryDTO> GetSummaryAsync(Guid matchId);
        Task<MatchWorkspaceDTO> GetWorkspaceAsync(Guid matchId);
    }
}
