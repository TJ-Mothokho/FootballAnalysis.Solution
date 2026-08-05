using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Domain.Models
{
    public class PlayerMatchStats
    {
        public int Id { get; set; }

        public int MatchId { get; set; }

        public Match Match { get; set; } = null!;

        public int PlayerId { get; set; }

        public Player Player { get; set; } = null!;

        public int TeamId { get; set; }

        public Team Team { get; set; } = null!;

        public bool Started { get; set; }

        public bool WasSubstitutedOn { get; set; }

        public bool WasSubstitutedOff { get; set; }

        public bool IsCaptain { get; set; }

        public bool IsManOfTheMatch { get; set; }

        public decimal Rating { get; set; }

        public int MinutesPlayed { get; set; }

        public int Goals { get; set; }

        public int Assists { get; set; }

        public double XG { get; set; }

        public double XGOT { get; set; }

        public double XA { get; set; }

        public double XGPlusXA { get; set; }

        public int Touches { get; set; }

        public int PassesCompleted { get; set; }

        public int PassesAttempted { get; set; }

        public double PassAccuracy => PassesAttempted == 0 ? 0 : (double)PassesCompleted / PassesAttempted * 100;

        public int ChancesCreated { get; set; }

        public int BigChancesCreated { get; set; }

        public int ShotsOnTarget { get; set; }

        public int ShotsOffTarget { get; set; }

        public int BlockedShots { get; set; }

        public int Tackles { get; set; }

        public int Blocks { get; set; }

        public int Clearances { get; set; }

        public int Interceptions { get; set; }

        public int Recoveries { get; set; }

        public int Saves { get; set; }

        public int GoalsConceded { get; set; }

        public string PerformanceSummary { get; set; } = "";

        public string AnalystNotes { get; set; } = "";
        
    }
}
