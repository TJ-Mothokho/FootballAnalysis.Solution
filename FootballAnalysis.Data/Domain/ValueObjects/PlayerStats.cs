using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Domain.ValueObjects
{
    public class PlayerStats
    {
        public double FotmobRating { get; set; } = 6.0;
        public double SofascoreRating { get; set; } = 6.0;
        public int MinutesPlayed { get; set; } = 90;
    }

    public class PlayerAttack
    {
        public int Goals { get; set; } = 0;
        public double XG { get; set; } = 0.0;
        public double XGOT { get; set; } = 0.0;
        public int TotalShots { get; set; } = 0;
        public int ShotsOnTarget { get; set; } = 0;
        public int TouchesInOppositionBox { get; set; } = 0;
        public int BigChancesMissed { get; set; } = 0;
        public int SuccessfulDribbles { get; set; } = 0;
        public int DribblesAttempted { get; set; } = 0;
    }
    public class PlayerPasses
    {
        public int Touches { get; set; } = 0;
        public int AccuratePasses { get; set; } = 0;
        public int PassesAttempted { get; set; } = 0;
        public int Assists { get; set; } = 0;
        public double XA { get; set; } = 0.0;
        public int ChancesCreated { get; set; } = 0;
        public int PassesIntoFinalThird { get; set; } = 0;
        public int AccurateCrosses { get; set; } = 0;
        public int CrossesAttempted { get; set; } = 0;
        public int AccurateLongBalls { get; set; } = 0;
        public int LongBallsAttempted { get; set; } = 0;
    }
    public class PlayerDefence
    {
        public int DefensiveContributions { get; set; } = 0;
        public int Tackles { get; set; } = 0;
        public int Interceptions { get; set; } = 0;
        public int Blocks { get; set; } = 0;
        public int Recoveries { get; set; } = 0;
        public int Clearance { get; set; } = 0;
        public int HeadedClearances { get; set; } = 0;
        public int DribbledPast { get; set; } = 0;
    }
    public class PlayerDuels
    {
        public int DuelsWon { get; set; } = 0;
        public int DuelsLost { get; set; } = 0;
        public int GroundDuelsWon { get; set; } = 0;
        public int TotalGroundDuels { get; set; } = 0;
        public int AerialDuelsWon { get; set; } = 0;
        public int TotalAerialDuels { get; set; } = 0;
    }
    public class Goalkeepering
    {
        public int Saves { get; set; } = 0;
        public int GoalsConceded { get; set; } = 0;
        public double FacedxGOT { get; set; } = 0.0;
        public double GoalsPrevented { get; set; } = 0.0;
        public int ActedAsSweeper { get; set; } = 0;
        public int HighClaim { get; set; } = 0;
        public int LongBalls { get; set; } = 0;
        public int AccurateLongBalls { get; set; } = 0;
        public int Passes { get; set; } = 0;
        public int AccuratePasses { get; set; } = 0;
    }

    public class PlayerDiscipline
    {
        public int YellowCards { get; set; } = 0;
        public int RedCards { get; set; } = 0;
        public int FoulsCommitted { get; set; } = 0;
        public int WasFouled { get; set; } = 0;
    }

    public class PlayerAnalysis
    {
        public string PerformanceSummary { get; set; } = "";

        public string AnalystNotes { get; set; } = "";
    }
}
