using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Domain.ValueObjects
{
    public class MatchStats
    {
        public int Possession { get; set; } = 0;
        public int Corners { get; set; } = 0;
        public int BigChances { get; set; } = 0;
        public int BigChancesMissed { get; set; } = 0;
    }

    public class MatchShots
    {
        public int TotalShots { get; set; } = 0;
        public int ShotsOnTarget { get; set; } = 0;
        public int ShotsOffTarget { get; set; } = 0;
        public int BlockedShots { get; set; } = 0;
        public int HitWoodwork { get; set; } = 0;
        public int ShotsInsideBox { get; set; } = 0;
        public int ShotsOutsideBox { get; set; } = 0;
    }

    public class MatchExpectedGoals
    {
        public double XG { get; set; } = 0.0;
        public double OpenPlayXG { get; set; } = 0.0;
        public double SetPlayXG { get; set; } = 0.0;
        public double NonPenaltyXG { get; set; } = 0.0;
        public double XGOT { get; set; } = 0.0;
    }

    public class MatchPasses
    {
        public int Passes { get; set; } = 0;
        public int AccuratePasses { get; set; } = 0;
        public int OwnHalf { get; set; } = 0;
        public int OppositionHalf { get; set; } = 0;
        public int AccurateLongBalls { get; set; } = 0;
        public int AccurateLongBallsPercentage { get; set; } = 0;
        public int AccurateCrosses { get; set; } = 0;
        public int AccurateCrossesPercentage { get; set; } = 0;
        public int Throws { get; set; } = 0;
        public int TouchesInOppositionBox { get; set; } = 0;
        public int Offsides { get; set; } = 0;
    }

    public class MatchDiscipline
    {
        public int FoulsCommitted { get; set; } = 0;
        public int YellowCards { get; set; } = 0;
        public int RedCards { get; set; } = 0;
    }

    public class MatchDefence
    {
        public int Tackles { get; set; } = 0;
        public int Interceptions { get; set; } = 0;
        public int Blocks { get; set; } = 0;
        public int Clearances { get; set; } = 0;
        public int KeeperSaves { get; set; } = 0;
    }

    public class MatchDuels
    {
        public int DuelsWon { get; set; } = 0;

        public int GroundDuelsWon { get; set; } = 0;
        public int GroundDuelsWonPercentage { get; set; } = 0;

        public int AerialDuelsWon { get; set; } = 0;
        public int AerialDuelsWonPercentage { get; set; } = 0;

        public int SuccessfulDribbles { get; set; } = 0;
        public int SuccessfulDribblesPercentage { get; set; } = 0;
    }

    public class MatchAttackingZones
    {
        public int CenterAttack { get; set; } = 0;
        public int LeftAttack { get; set; } = 0;
        public int RightAttack { get; set; } = 0;
    }

    public class MatchAnalysis
    {
        public string TacticalNotes { get; set; } = "";
        public string AnalystNotes { get; set; } = "";
    }
}
