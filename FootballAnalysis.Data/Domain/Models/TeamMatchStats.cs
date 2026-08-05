using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Domain.Models
{
    public class TeamMatchStats
    {
        public int Id { get; set; }

        public int MatchId { get; set; }

        public Match Match { get; set; } = null!;

        public int TeamId { get; set; }

        public Team Team { get; set; } = null!;

        public bool IsHome { get; set; }

        public string Formation { get; set; } = "";

        public string PlayingStyle { get; set; } = "";

        public double XG { get; set; }

        public int Possession { get; set; }

        public int Shots { get; set; }

        public int ShotsOnTarget { get; set; }

        public int ShotsOffTarget { get; set; }

        public int BlockedShots { get; set; }

        public int BigChances { get; set; }

        public int Corners { get; set; }

        public int Offsides { get; set; }

        public int Fouls { get; set; }

        public int YellowCards { get; set; }

        public int RedCards { get; set; }

        public int PassesCompleted { get; set; }

        public int PassesAttempted { get; set; }

        public int CrossesCompleted { get; set; }

        public int CrossesAttempted { get; set; }

        public int LongBallsCompleted { get; set; }

        public int LongBallsAttempted { get; set; }

        public int Tackles { get; set; }

        public int Clearances { get; set; }

        public int Interceptions { get; set; }

        public int Recoveries { get; set; }

        public int Saves { get; set; }

        public string TacticalNotes { get; set; } = "";

        public string AnalystNotes { get; set; } = "";
    }
}
