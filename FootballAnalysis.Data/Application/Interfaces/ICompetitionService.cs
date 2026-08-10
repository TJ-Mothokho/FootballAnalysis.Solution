using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Competition;
using FootballAnalysis.Data.Application.DTOs.Match;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface ICompetitionService : IService<GetCompetitionDTO, CreateCompetitionDTO, UpdateCompetitionDTO>
    {
        Task<IEnumerable<StandingDTO>> GetStandingsAsync(Guid competitionId, Guid seasonId);
        Task<IEnumerable<GetMatchDTO>> GetFixturesAsync(Guid competitionId);
        Task<IEnumerable<GetMatchDTO>> GetResultsAsync(Guid competitionId);
        Task<IEnumerable<PlayerLeaderDTO>> GetTopScorersAsync(Guid competitionId);
        Task<CompetitionStatisticsDTO> GetStatisticsAsync(Guid competitionId);
    }
}
