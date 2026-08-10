using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Player;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Application.LogExceptions;
using FootballAnalysis.Data.Application.Mappings;
using FootballAnalysis.Data.Application.Validations.Team;
using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Domain.Models;
using FootballAnalysis.Data.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace FootballAnalysis.Data.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly ApplicationDbContext _context;

        public TeamService(ITeamRepository teamRepository, IPlayerRepository playerRepository, ApplicationDbContext context)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _context = context;
        }

        public async Task<GetTeamDTO> CreateAsync(CreateTeamDTO entity)
        {
            try
            {
                var validatorResult = await new CreateTeamValidator().ValidateAsync(entity);
                if (!validatorResult.IsValid) throw new ArgumentException("Invalid team data.");

                var team = TeamMap.ToDomainModel(entity);
                var createdTeam = await _teamRepository.AddAsync(team);
                return TeamMap.ToGetDTO(createdTeam);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while creating a team.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _teamRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while deleting a team.", ex);
            }
        }

        public async Task<IEnumerable<GetTeamDTO>> GetAllAsync()
        {
            try
            {
                var teams = await _context.Teams.AsNoTracking().ToListAsync();
                return teams.Select(TeamMap.ToGetDTO);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching teams.", ex);
            }
        }

        public async Task<GetTeamDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var team = await _context.Teams.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
                return team == null ? throw new KeyNotFoundException("Team not found.") : TeamMap.ToGetDTO(team);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching a team.", ex);
            }
        }

        public async Task<IEnumerable<GetMatchDTO>> GetMatchesAsync(Guid teamId)
        {
            await EnsureTeamExists(teamId);
            var matches = await TeamMatches(teamId).OrderByDescending(m => m.KickOff).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetPlayerDTO>> GetPlayersAsync(Guid teamId)
        {
            await EnsureTeamExists(teamId);
            var players = await _context.Players.AsNoTracking().Where(p => p.TeamId == teamId).OrderBy(p => p.ShirtNumber).ToListAsync();
            return players.Select(PlayerMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetMatchDTO>> GetFixturesAsync(Guid teamId)
        {
            await EnsureTeamExists(teamId);
            var matches = await TeamMatches(teamId).Where(m => m.KickOff > DateTime.Now).OrderBy(m => m.KickOff).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetMatchDTO>> GetResultsAsync(Guid teamId)
        {
            await EnsureTeamExists(teamId);
            var matches = await TeamMatches(teamId).Where(m => m.KickOff <= DateTime.Now).OrderByDescending(m => m.KickOff).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<GetMatchDTO>> GetLast5MatchesAsync(Guid teamId)
        {
            await EnsureTeamExists(teamId);
            var matches = await TeamMatches(teamId).Where(m => m.KickOff <= DateTime.Now).OrderByDescending(m => m.KickOff).Take(5).ToListAsync();
            return matches.Select(MatchMap.ToGetDTO);
        }

        public async Task<IEnumerable<string>> GetFormAsync(Guid teamId)
        {
            await EnsureTeamExists(teamId);
            var stats = await _context.TeamMatchStats
                .AsNoTracking()
                .Include(tms => tms.Match)
                .Where(tms => tms.TeamId == teamId && tms.Match.KickOff <= DateTime.Now)
                .OrderByDescending(tms => tms.Match.KickOff)
                .Take(5)
                .ToListAsync();

            return stats.Select(ResultLetter);
        }

        public async Task<GetTeamStatisticsDTO> GetStatisticsAsync(Guid teamId)
        {
            await EnsureTeamExists(teamId);
            var teamStats = await _context.TeamMatchStats
                .AsNoTracking()
                .Include(tms => tms.Match)
                .Where(tms => tms.TeamId == teamId)
                .ToListAsync();

            var playerStats = await _context.PlayerMatchStats
                .AsNoTracking()
                .Include(pms => pms.Player)
                .Where(pms => pms.TeamId == teamId)
                .ToListAsync();

            var matchesPlayed = teamStats.Count;
            var shots = teamStats.Sum(tms => tms.MatchShots.TotalShots);
            var shotsOnTarget = teamStats.Sum(tms => tms.MatchShots.ShotsOnTarget);
            var passesAttempted = teamStats.Sum(tms => tms.MatchPasses.Passes);
            var passesCompleted = teamStats.Sum(tms => tms.MatchPasses.AccuratePasses);
            var topScorer = PlayerLeader(playerStats, pms => pms.PlayerAttack.Goals).FirstOrDefault();
            var topAssister = PlayerLeader(playerStats, pms => pms.PlayerPasses.Assists).FirstOrDefault();
            var highestRated = RatedLeader(playerStats).FirstOrDefault();

            return new GetTeamStatisticsDTO
            {
                MatchesPlayed = matchesPlayed,
                Wins = teamStats.Count(tms => tms.MatchStats.TeamGoals > tms.MatchStats.OppositionGoals),
                Draws = teamStats.Count(tms => tms.MatchStats.TeamGoals == tms.MatchStats.OppositionGoals),
                Losses = teamStats.Count(tms => tms.MatchStats.TeamGoals < tms.MatchStats.OppositionGoals),
                GoalsScored = teamStats.Sum(tms => tms.MatchStats.TeamGoals),
                GoalsConceded = teamStats.Sum(tms => tms.MatchStats.OppositionGoals),
                GoalDifference = teamStats.Sum(tms => tms.MatchStats.TeamGoals) - teamStats.Sum(tms => tms.MatchStats.OppositionGoals),
                CleanSheets = teamStats.Count(tms => tms.MatchStats.OppositionGoals == 0),
                AverageGoalsPerMatch = Average(teamStats, tms => tms.MatchStats.TeamGoals),
                AverageXG = Average(teamStats, tms => tms.MatchExpectedGoals.XG),
                TotalShots = shots,
                ShotsOnTarget = shotsOnTarget,
                ShotsOffTarget = teamStats.Sum(tms => tms.MatchShots.ShotsOffTarget),
                BlockedShots = teamStats.Sum(tms => tms.MatchShots.BlockedShots),
                HitWoodwork = teamStats.Sum(tms => tms.MatchShots.HitWoodwork),
                ShotAccuracy = Percentage(shotsOnTarget, shots),
                Corners = teamStats.Sum(tms => tms.MatchStats.Corners),
                BigChancesCreated = teamStats.Sum(tms => tms.MatchStats.BigChances),
                BigChancesMissed = teamStats.Sum(tms => tms.MatchStats.BigChancesMissed),
                AveragePossession = Average(teamStats, tms => tms.MatchStats.Possession),
                TotalPassesCompleted = passesCompleted,
                TotalPassesAttempted = passesAttempted,
                PassAccuracy = Percentage(passesCompleted, passesAttempted),
                TotalCrossesCompleted = teamStats.Sum(tms => tms.MatchPasses.AccurateCrosses),
                CrossAccuracy = Average(teamStats, tms => tms.MatchPasses.AccurateCrossesPercentage),
                TotalLongBallsCompleted = teamStats.Sum(tms => tms.MatchPasses.AccurateLongBalls),
                LongBallsAccuracy = Average(teamStats, tms => tms.MatchPasses.AccurateLongBallsPercentage),
                Tackles = teamStats.Sum(tms => tms.MatchDefence.Tackles),
                Interceptions = teamStats.Sum(tms => tms.MatchDefence.Interceptions),
                Blocks = teamStats.Sum(tms => tms.MatchDefence.Blocks),
                Clearances = teamStats.Sum(tms => tms.MatchDefence.Clearances),
                KeeperSaves = teamStats.Sum(tms => tms.MatchDefence.KeeperSaves),
                DuelsWon = teamStats.Sum(tms => tms.MatchDuels.DuelsWon),
                GroundDuelsWon = teamStats.Sum(tms => tms.MatchDuels.GroundDuelsWon),
                GroundDuelsWonPercentage = Average(teamStats, tms => tms.MatchDuels.GroundDuelsWonPercentage),
                AerialDuelsWon = teamStats.Sum(tms => tms.MatchDuels.AerialDuelsWon),
                AerialDuelsWonPercentage = Average(teamStats, tms => tms.MatchDuels.AerialDuelsWonPercentage),
                SuccessfulDribbles = teamStats.Sum(tms => tms.MatchDuels.SuccessfulDribbles),
                SuccessfulDribblesPercentage = Average(teamStats, tms => tms.MatchDuels.SuccessfulDribblesPercentage),
                CenterAttack = teamStats.Sum(tms => tms.MatchAttackingZones.CenterAttack),
                LeftAttack = teamStats.Sum(tms => tms.MatchAttackingZones.LeftAttack),
                RightAttack = teamStats.Sum(tms => tms.MatchAttackingZones.RightAttack),
                FoulsCommitted = teamStats.Sum(tms => tms.MatchDiscipline.FoulsCommitted),
                YellowCards = teamStats.Sum(tms => tms.MatchDiscipline.YellowCards),
                RedCards = teamStats.Sum(tms => tms.MatchDiscipline.RedCards),
                TopScorer = topScorer == null ? null : $"{topScorer.FirstName} {topScorer.LastName}",
                TopScorerGoals = topScorer?.Total ?? 0,
                TopAssistProvider = topAssister == null ? null : $"{topAssister.FirstName} {topAssister.LastName}",
                TopAssists = topAssister?.Total ?? 0,
                HighestRatedPlayer = highestRated == null ? null : $"{highestRated.FirstName} {highestRated.LastName}",
                HighestAverageRating = highestRated?.AverageRating ?? 0,
                LastFiveResults = (await GetFormAsync(teamId)).ToList()
            };
        }

        public async Task<IEnumerable<PlayerLeaderDTO>> GetTopScorersAsync(Guid teamId) => await TeamPlayerLeaders(teamId, pms => pms.PlayerAttack.Goals);

        public async Task<IEnumerable<PlayerLeaderDTO>> GetTopAssistsAsync(Guid teamId) => await TeamPlayerLeaders(teamId, pms => pms.PlayerPasses.Assists);

        public async Task<IEnumerable<PlayerLeaderDTO>> GetTopRatedAsync(Guid teamId)
        {
            await EnsureTeamExists(teamId);
            var stats = await TeamPlayerStats(teamId).ToListAsync();
            return RatedLeader(stats);
        }

        public async Task<IEnumerable<PlayerLeaderDTO>> GetMostPassesAsync(Guid teamId) => await TeamPlayerLeaders(teamId, pms => pms.PlayerPasses.AccuratePasses);

        public async Task<IEnumerable<PlayerLeaderDTO>> GetMostMinutesAsync(Guid teamId) => await TeamPlayerLeaders(teamId, pms => pms.MinutesPlayed);

        public async Task<GetTeamDTO> UpdateAsync(Guid id, UpdateTeamDTO entity)
        {
            try
            {
                var validatorResult = await new UpdateTeamValidator().ValidateAsync(entity);
                if (!validatorResult.IsValid) throw new ArgumentException("Invalid team data.");

                var existingTeam = await _teamRepository.GetAsync(id);
                if (entity.FoundedYear < 0) throw new ArgumentException("Founded year cannot be negative.");
                if (entity.Captain != null) existingTeam.Captain = await _playerRepository.GetAsync(entity.Captain);

                existingTeam.Name = entity.Name ?? existingTeam.Name;
                existingTeam.ShortName = entity.ShortName ?? existingTeam.ShortName;
                existingTeam.Stadium = entity.Stadium ?? existingTeam.Stadium;
                existingTeam.City = entity.City ?? existingTeam.City;
                existingTeam.FoundedYear = entity.FoundedYear > 0 ? entity.FoundedYear : existingTeam.FoundedYear;
                existingTeam.Coach = entity.Coach ?? existingTeam.Coach;
                existingTeam.PreferredFormation = entity.PreferredFormation ?? existingTeam.PreferredFormation;
                existingTeam.PlayingStyle = entity.PlayingStyle ?? existingTeam.PlayingStyle;

                var updatedTeam = await _teamRepository.UpdateAsync(existingTeam);
                return TeamMap.ToGetDTO(updatedTeam);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while updating a team.", ex);
            }
        }

        private IQueryable<Match> TeamMatches(Guid teamId)
        {
            return _context.Matches
                .AsNoTracking()
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.Competition)
                .Include(m => m.Season)
                .Where(m => m.HomeTeamId == teamId || m.AwayTeamId == teamId);
        }

        private IQueryable<PlayerMatchStats> TeamPlayerStats(Guid teamId)
        {
            return _context.PlayerMatchStats
                .AsNoTracking()
                .Include(pms => pms.Player)
                .ThenInclude(p => p.Team)
                .Where(pms => pms.TeamId == teamId);
        }

        private async Task<IEnumerable<PlayerLeaderDTO>> TeamPlayerLeaders(Guid teamId, Func<PlayerMatchStats, int> selector)
        {
            await EnsureTeamExists(teamId);
            var stats = await TeamPlayerStats(teamId).ToListAsync();
            return PlayerLeader(stats, selector);
        }

        private static IEnumerable<PlayerLeaderDTO> PlayerLeader(IEnumerable<PlayerMatchStats> stats, Func<PlayerMatchStats, int> selector)
        {
            return stats
                .GroupBy(pms => pms.Player)
                .Select(group => new PlayerLeaderDTO(
                    group.Key.Id,
                    group.Key.FirstName,
                    group.Key.LastName,
                    group.Key.Team?.Name ?? "",
                    group.Sum(selector),
                    Average(group, pms => pms.PlayerStats.FotmobRating)))
                .OrderByDescending(player => player.Total)
                .ThenByDescending(player => player.AverageRating)
                .Take(10);
        }

        private static IEnumerable<PlayerLeaderDTO> RatedLeader(IEnumerable<PlayerMatchStats> stats)
        {
            return stats
                .GroupBy(pms => pms.Player)
                .Select(group => new PlayerLeaderDTO(
                    group.Key.Id,
                    group.Key.FirstName,
                    group.Key.LastName,
                    group.Key.Team?.Name ?? "",
                    group.Count(),
                    Average(group, pms => pms.PlayerStats.FotmobRating)))
                .OrderByDescending(player => player.AverageRating)
                .Take(10);
        }

        private async Task EnsureTeamExists(Guid teamId)
        {
            if (!await _context.Teams.AsNoTracking().AnyAsync(t => t.Id == teamId))
            {
                throw new KeyNotFoundException("Team not found.");
            }
        }

        private static string ResultLetter(TeamMatchStats stats)
        {
            if (stats.MatchStats.TeamGoals > stats.MatchStats.OppositionGoals) return "W";
            if (stats.MatchStats.TeamGoals < stats.MatchStats.OppositionGoals) return "L";
            return "D";
        }

        private static double Percentage(int numerator, int denominator) => denominator == 0 ? 0 : (double)numerator / denominator * 100;

        private static double Average<T>(IEnumerable<T> source, Func<T, int> selector) => source.Any() ? source.Average(selector) : 0;

        private static double Average<T>(IEnumerable<T> source, Func<T, double> selector) => source.Any() ? source.Average(selector) : 0;
    }
}
