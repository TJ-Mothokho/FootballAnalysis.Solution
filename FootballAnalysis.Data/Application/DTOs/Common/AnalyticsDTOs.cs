using FootballAnalysis.Data.Application.DTOs.Competition;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Player;
using FootballAnalysis.Data.Application.DTOs.PlayerMatchStats;
using FootballAnalysis.Data.Application.DTOs.Season;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.DTOs.TeamMatchStats;

namespace FootballAnalysis.Data.Application.DTOs.Common
{
    public record PlayerLeaderDTO(
        Guid PlayerId,
        string FirstName,
        string LastName,
        string TeamName,
        int Total,
        double AverageRating);

    public record PlayerStatisticsDTO(
        Guid PlayerId,
        string FirstName,
        string LastName,
        int MatchesPlayed,
        int Starts,
        int MinutesPlayed,
        int Goals,
        int Assists,
        double AverageFotmobRating,
        double AverageSofascoreRating,
        int TotalShots,
        int ShotsOnTarget,
        double ShotAccuracy,
        int AccuratePasses,
        int PassesAttempted,
        double PassAccuracy,
        int ChancesCreated,
        int Tackles,
        int Interceptions,
        int Recoveries,
        int Saves,
        int YellowCards,
        int RedCards);

    public record PlayerComparisonDTO(PlayerStatisticsDTO Player, PlayerStatisticsDTO OtherPlayer);

    public record MatchSummaryDTO(
        GetMatchDTO Match,
        string Scoreline,
        GetTeamMatchStatsDTO? HomeTeamStats,
        GetTeamMatchStatsDTO? AwayTeamStats,
        IEnumerable<GetPlayerMatchStatsDTO> TopRatedPlayers,
        IEnumerable<GetPlayerMatchStatsDTO> GoalScorers,
        IEnumerable<GetPlayerMatchStatsDTO> AssistProviders);

    public record MatchWorkspaceDTO(
        GetMatchDTO Match,
        GetTeamDTO HomeTeam,
        GetTeamDTO AwayTeam,
        GetCompetitionDTO Competition,
        GetSeasonDTO Season,
        GetTeamMatchStatsDTO? HomeTeamStats,
        GetTeamMatchStatsDTO? AwayTeamStats,
        IEnumerable<GetPlayerMatchStatsDTO> HomePlayers,
        IEnumerable<GetPlayerMatchStatsDTO> AwayPlayers);

    public record CompetitionStatisticsDTO(
        Guid CompetitionId,
        string CompetitionName,
        int Matches,
        int CompletedMatches,
        int UpcomingMatches,
        int Goals,
        double AverageGoalsPerMatch);

    public record StandingDTO(
        int Position,
        Guid TeamId,
        string TeamName,
        int Played,
        int Wins,
        int Draws,
        int Losses,
        int GoalsFor,
        int GoalsAgainst,
        int GoalDifference,
        int Points);

    public record SeasonStatisticsDTO(
        Guid SeasonId,
        string SeasonName,
        int Matches,
        int CompletedMatches,
        int UpcomingMatches,
        int Teams,
        int Players,
        int Goals,
        double AverageGoalsPerMatch);

    public record DashboardOverviewDTO(
        int Competitions,
        int Seasons,
        int Teams,
        int Players,
        int Matches,
        int CompletedMatches,
        int UpcomingMatches,
        int Goals);

    public record GoalsPerRoundDTO(DateOnly Date, int Goals);

    public record SearchResultDTO(string Type, Guid Id, string Name);
}
