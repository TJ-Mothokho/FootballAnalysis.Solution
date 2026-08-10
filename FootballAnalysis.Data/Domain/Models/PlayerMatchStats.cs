using FootballAnalysis.Data.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Domain.Models
{
    public class PlayerMatchStats
    {
        public Guid Id { get; set; }

        public Guid MatchId { get; set; }

        public Match Match { get; set; } = null!;

        public Guid PlayerId { get; set; }

        public Player Player { get; set; } = null!;

        public Guid TeamId { get; set; }

        public Team Team { get; set; } = null!;

        public bool Started { get; set; }

        public bool WasSubstitutedOn { get; set; }

        public bool WasSubstitutedOff { get; set; }
        public int MinutesPlayed { get; set; }

        public bool IsCaptain { get; set; }

        public bool IsManOfTheMatch { get; set; }

        public PlayerAnalysis Analysis { get; set; } = null!;
        public PlayerStats PlayerStats { get; set; } = null!;
        public PlayerAttack PlayerAttack { get; set; } = null!;
        public PlayerPasses PlayerPasses { get; set; } = null!;
        public PlayerDefence PlayerDefence { get; set; } = null!;
        public PlayerDuels PlayerDuels { get; set; } = null!;
        public Goalkeepering Goalkeepering { get; set; } = null!;
        public PlayerDiscipline PlayerDiscipline { get; set; } = null!;

    }
}
