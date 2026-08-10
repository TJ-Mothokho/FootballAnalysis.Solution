using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Player;
using FootballAnalysis.Data.Application.DTOs.Season;
using FootballAnalysis.Data.Application.DTOs.Team;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface ISeasonService : IService<GetSeasonDTO, CreateSeasonDTO, UpdateSeasonDTO>
    {
        Task<GetSeasonStatisticsDTO> GetSeasonStatisticsAsync(Guid id);
        Task<IEnumerable<GetMatchDTO>> GetSeasonMatchesAsync(Guid id);
        Task<IEnumerable<GetPlayerDTO>> GetSeasonPlayersAsync(Guid id);
        Task<IEnumerable<GetTeamDTO>> GetSeasonTeamsAsync(Guid id);
        Task<IEnumerable<PlayerLeaderDTO>> GetSeasonLeadersAsync(Guid id);
    }
}
