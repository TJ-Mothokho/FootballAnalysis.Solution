using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Player;
using FootballAnalysis.Data.Application.DTOs.Team;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface ITeamService : IService<GetTeamDTO, CreateTeamDTO, UpdateTeamDTO>
    {
        Task<IEnumerable<GetMatchDTO>> GetMatchesAsync(Guid teamId);
        Task<IEnumerable<GetPlayerDTO>> GetPlayersAsync(Guid teamId);
        Task<IEnumerable<GetMatchDTO>> GetFixturesAsync(Guid teamId);
        Task<IEnumerable<GetMatchDTO>> GetResultsAsync(Guid teamId);
        Task<IEnumerable<GetMatchDTO>> GetLast5MatchesAsync(Guid teamId);
        Task<IEnumerable<string>> GetFormAsync(Guid teamId);
        Task<GetTeamStatisticsDTO> GetStatisticsAsync(Guid teamId);
        Task<IEnumerable<PlayerLeaderDTO>> GetTopScorersAsync(Guid teamId);
        Task<IEnumerable<PlayerLeaderDTO>> GetTopAssistsAsync(Guid teamId);
        Task<IEnumerable<PlayerLeaderDTO>> GetTopRatedAsync(Guid teamId);
        Task<IEnumerable<PlayerLeaderDTO>> GetMostPassesAsync(Guid teamId);
        Task<IEnumerable<PlayerLeaderDTO>> GetMostMinutesAsync(Guid teamId);
    }
}
