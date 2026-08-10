using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FootballAnalysis.Data.Application.Services
{
    public class SearchService : ISearchService
    {
        private readonly ApplicationDbContext _context;

        public SearchService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SearchResultDTO>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query)) return [];

            var normalized = query.Trim();
            var teams = await _context.Teams.AsNoTracking()
                .Where(t => t.Name.Contains(normalized) || t.ShortName.Contains(normalized) || t.City.Contains(normalized))
                .Select(t => new SearchResultDTO("Team", t.Id, t.Name))
                .ToListAsync();

            var players = await _context.Players.AsNoTracking()
                .Where(p => p.FirstName.Contains(normalized) || p.LastName.Contains(normalized) || (p.FirstName + " " + p.LastName).Contains(normalized))
                .Select(p => new SearchResultDTO("Player", p.Id, p.FirstName + " " + p.LastName))
                .ToListAsync();

            var competitions = await _context.Competitions.AsNoTracking()
                .Where(c => c.Name.Contains(normalized) || c.Country.Contains(normalized))
                .Select(c => new SearchResultDTO("Competition", c.Id, c.Name))
                .ToListAsync();

            var seasons = await _context.Seasons.AsNoTracking()
                .Where(s => s.Name.Contains(normalized))
                .Select(s => new SearchResultDTO("Season", s.Id, s.Name))
                .ToListAsync();

            return teams.Concat(players).Concat(competitions).Concat(seasons).Take(25);
        }
    }
}
