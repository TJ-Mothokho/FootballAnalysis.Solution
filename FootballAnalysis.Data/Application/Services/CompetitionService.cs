using FluentValidation;
using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Competition;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Application.LogExceptions;
using FootballAnalysis.Data.Application.Mappings;
using FootballAnalysis.Data.Application.Validations.Competition;
using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Domain.Models;
using FootballAnalysis.Data.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FootballAnalysis.Data.Application.Services
{
    public class CompetitionService : ICompetitionService
    {
        private readonly ICompetitionRepository _competitionRepository;
        private readonly ApplicationDbContext _context;

        public CompetitionService(ICompetitionRepository competitionRepository, ApplicationDbContext context)
        {
            _competitionRepository = competitionRepository;
            _context = context;
        }

        public async Task<GetCompetitionDTO> CreateAsync(CreateCompetitionDTO competitionDto)
        {
            try
            {
                var validationResult = await new CreateCompetitionValidator().ValidateAsync(competitionDto);
                if (!validationResult.IsValid) throw new ValidationException("Invalid competition data.", validationResult.Errors);

                var competition = new Competition
                {
                    Id = Guid.NewGuid(),
                    Name = competitionDto.Name,
                    Country = competitionDto.Country
                };

                var createdCompetition = await _competitionRepository.AddAsync(competition);
                return CompetitionMap.ToGetDTO(createdCompetition);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new ApplicationException("An error occurred while adding competition.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var competition = await _competitionRepository.GetAsync(id);
                await _competitionRepository.DeleteAsync(competition);
                return true;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new ApplicationException("An error occurred while deleting competition.", ex);
            }
        }

        public async Task<IEnumerable<GetCompetitionDTO>> GetAllAsync()
        {
            try
            {
                var competitions = await _context.Competitions.AsNoTracking().ToListAsync();
                return competitions.Select(CompetitionMap.ToGetDTO);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new ApplicationException("An error occurred while retrieving competitions.", ex);
            }
        }

        public async Task<GetCompetitionDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var competition = await _context.Competitions.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
                return competition == null ? throw new KeyNotFoundException("Competition not found.") : CompetitionMap.ToGetDTO(competition);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new ApplicationException("An error occurred while retrieving competition.", ex);
            }
        }

        public async Task<IEnumerable<StandingDTO>> GetStandingsAsync(Guid competitionId, Guid seasonId)
        {
            await EnsureCompetitionExists(competitionId);
            await EnsureSeasonExists(seasonId);

            var matches = await _context.Matches
                .AsNoTracking()
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Where(m => m.CompetitionId == competitionId && m.SeasonId == seasonId)
                .ToListAsync();

            var rows = new Dictionary<Guid, StandingAccumulator>();
            foreach (var match in matches.Where(IsCompletedMatch))
            {
                ApplyMatch(rows, match.HomeTeamId, match.HomeTeam.Name, match.HomeGoals, match.AwayGoals);
                ApplyMatch(rows, match.AwayTeamId, match.AwayTeam.Name, match.AwayGoals, match.HomeGoals);
            }

            return rows.Values
                .Select(row => new StandingDTO(
                    0,
                    row.TeamId,
                    row.TeamName,
                    row.Played,
                    row.Wins,
                    row.Draws,
                    row.Losses,
                    row.GoalsFor,
                    row.GoalsAgainst,
                    row.GoalsFor - row.GoalsAgainst,
                    row.Wins * 3 + row.Draws))
                .OrderByDescending(row => row.Points)
                .ThenByDescending(row => row.GoalDifference)
                .ThenByDescending(row => row.GoalsFor)
                .Select((row, index) => row with { Position = index + 1 });
        }

        public async Task<IEnumerable<GetMatchDTO>> GetFixturesAsync(Guid competitionId)
        {
            await EnsureCompetitionExists(competitionId);
            var matches = await MatchGraph().Where(m => m.CompetitionId == competitionId && m.KickOff > DateTime.Now).OrderBy(m => m.KickOff).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetMatchDTO>> GetResultsAsync(Guid competitionId)
        {
            await EnsureCompetitionExists(competitionId);
            var matches = await MatchGraph().Where(m => m.CompetitionId == competitionId && m.KickOff <= DateTime.Now).OrderByDescending(m => m.KickOff).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<PlayerLeaderDTO>> GetTopScorersAsync(Guid competitionId)
        {
            await EnsureCompetitionExists(competitionId);
            var stats = await _context.PlayerMatchStats
                .AsNoTracking()
                .Include(pms => pms.Player)
                .ThenInclude(p => p.Team)
                .Include(pms => pms.Match)
                .Where(pms => pms.Match.CompetitionId == competitionId)
                .ToListAsync();

            return stats
                .GroupBy(pms => pms.Player)
                .Select(group => new PlayerLeaderDTO(group.Key.Id, group.Key.FirstName, group.Key.LastName, group.Key.Team?.Name ?? "", group.Sum(pms => pms.PlayerAttack.Goals), group.Any() ? group.Average(pms => pms.PlayerStats.FotmobRating) : 0))
                .OrderByDescending(player => player.Total)
                .Take(10);
        }

        public async Task<CompetitionStatisticsDTO> GetStatisticsAsync(Guid competitionId)
        {
            var competition = await _context.Competitions.AsNoTracking().FirstOrDefaultAsync(c => c.Id == competitionId);
            if (competition == null) throw new KeyNotFoundException("Competition not found.");

            var matches = await _context.Matches.AsNoTracking().Where(m => m.CompetitionId == competitionId).ToListAsync();
            var completed = matches.Count(IsCompletedMatch);
            var goals = matches.Where(IsCompletedMatch).Sum(m => m.HomeGoals + m.AwayGoals);
            return new CompetitionStatisticsDTO(competition.Id, competition.Name, matches.Count, completed, matches.Count - completed, goals, completed == 0 ? 0 : (double)goals / completed);
        }

        public async Task<GetCompetitionDTO> UpdateAsync(Guid id, UpdateCompetitionDTO competitionDto)
        {
            try
            {
                var validationResult = await new UpdateCompetitionValidator().ValidateAsync(competitionDto);
                if (!validationResult.IsValid) throw new ValidationException("Invalid competition data.", validationResult.Errors);

                var existingCompetition = await _competitionRepository.GetAsync(id);
                existingCompetition.Name = competitionDto.Name ?? existingCompetition.Name;
                existingCompetition.Country = competitionDto.Country ?? existingCompetition.Country;

                var updatedCompetition = await _competitionRepository.UpdateAsync(existingCompetition);
                return CompetitionMap.ToGetDTO(updatedCompetition);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new ApplicationException("An error occurred while updating competition.", ex);
            }
        }

        private IQueryable<Match> MatchGraph()
        {
            return _context.Matches
                .AsNoTracking()
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.Competition)
                .Include(m => m.Season);
        }

        private async Task EnsureCompetitionExists(Guid competitionId)
        {
            if (!await _context.Competitions.AsNoTracking().AnyAsync(c => c.Id == competitionId))
            {
                throw new KeyNotFoundException("Competition not found.");
            }
        }

        private async Task EnsureSeasonExists(Guid seasonId)
        {
            if (!await _context.Seasons.AsNoTracking().AnyAsync(s => s.Id == seasonId))
            {
                throw new KeyNotFoundException("Season not found.");
            }
        }

        private static void ApplyMatch(Dictionary<Guid, StandingAccumulator> rows, Guid teamId, string teamName, int goalsFor, int goalsAgainst)
        {
            if (!rows.TryGetValue(teamId, out var row))
            {
                row = new StandingAccumulator(teamId, teamName);
                rows.Add(teamId, row);
            }

            row.Played++;
            row.GoalsFor += goalsFor;
            row.GoalsAgainst += goalsAgainst;

            if (goalsFor > goalsAgainst)
            {
                row.Wins++;
            }
            else if (goalsFor == goalsAgainst)
            {
                row.Draws++;
            }
            else
            {
                row.Losses++;
            }
        }

        private static bool IsCompletedMatch(Match match)
        {
            if (match.KickOff > DateTime.Now)
            {
                return false;
            }

            var status = match.Status?.Trim().ToLowerInvariant() ?? "";
            if (status is "cancelled" or "canceled" or "postponed" or "abandoned")
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(status))
            {
                return true;
            }

            return status is "completed" or "complete" or "finished" or "fulltime" or "full-time" or "ft" or "played" or "result";
        }

        private sealed class StandingAccumulator
        {
            public StandingAccumulator(Guid teamId, string teamName)
            {
                TeamId = teamId;
                TeamName = teamName;
            }

            public Guid TeamId { get; }
            public string TeamName { get; }
            public int Played { get; set; }
            public int Wins { get; set; }
            public int Draws { get; set; }
            public int Losses { get; set; }
            public int GoalsFor { get; set; }
            public int GoalsAgainst { get; set; }
        }
    }
}
