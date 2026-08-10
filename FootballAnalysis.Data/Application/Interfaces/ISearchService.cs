using FootballAnalysis.Data.Application.DTOs.Common;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResultDTO>> SearchAsync(string query);
    }
}
