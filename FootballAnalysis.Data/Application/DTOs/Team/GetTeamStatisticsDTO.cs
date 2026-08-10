namespace FootballAnalysis.Data.Application.DTOs.Team
{
    public class GetTeamStatisticsDTO
    {
        public int MatchesPlayed { get; init; }
        public int Wins { get; init; }
        public int Draws { get; init; }
        public int Losses { get; init; }
        public int GoalsScored { get; init; }
        public int GoalsConceded { get; init; }
        public int GoalDifference { get; init; }
        public int CleanSheets { get; init; }
        public double AverageGoalsPerMatch { get; init; }
        public double AverageXG { get; init; }
        public int TotalShots { get; init; }
        public int ShotsOnTarget { get; init; }
        public int ShotsOffTarget { get; init; }
        public int BlockedShots { get; init; }
        public int HitWoodwork { get; init; }
        public double ShotAccuracy { get; init; }
        public int Corners { get; init; }
        public int BigChancesCreated { get; init; }
        public int BigChancesMissed { get; init; }
        public double AveragePossession { get; init; }
        public int TotalPassesCompleted { get; init; }
        public int TotalPassesAttempted { get; init; }
        public double PassAccuracy { get; init; }
        public int TotalCrossesCompleted { get; init; }
        public double CrossAccuracy { get; init; }
        public int TotalLongBallsCompleted { get; init; }
        public double LongBallsAccuracy { get; init; }
        public int Tackles { get; init; }
        public int Interceptions { get; init; }
        public int Blocks { get; init; }
        public int Clearances { get; init; }
        public int KeeperSaves { get; init; }
        public int DuelsWon { get; init; }
        public int GroundDuelsWon { get; init; }
        public double GroundDuelsWonPercentage { get; init; }
        public int AerialDuelsWon { get; init; }
        public double AerialDuelsWonPercentage { get; init; }
        public int SuccessfulDribbles { get; init; }
        public double SuccessfulDribblesPercentage { get; init; }
        public int CenterAttack { get; init; }
        public int LeftAttack { get; init; }
        public int RightAttack { get; init; }
        public int FoulsCommitted { get; init; }
        public int YellowCards { get; init; }
        public int RedCards { get; init; }
        public string? TopScorer { get; init; }
        public int TopScorerGoals { get; init; }
        public string? TopAssistProvider { get; init; }
        public int TopAssists { get; init; }
        public string? HighestRatedPlayer { get; init; }
        public double HighestAverageRating { get; init; }
        public List<string> LastFiveResults { get; init; } = [];
    }
}
