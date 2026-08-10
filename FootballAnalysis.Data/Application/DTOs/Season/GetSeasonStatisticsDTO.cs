namespace FootballAnalysis.Data.Application.DTOs.Season
{
    public class GetSeasonStatisticsDTO
    {
        public Guid SeasonId { get; init; }
        public string SeasonName { get; init; } = "";
        public int Matches { get; init; }
        public int CompletedMatches { get; init; }
        public int UpcomingMatches { get; init; }
        public int Teams { get; init; }
        public int Players { get; init; }
        public int Goals { get; init; }
        public double AverageGoalsPerMatch { get; init; }
    }
}
