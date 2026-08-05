using FootballAnalysis.Data.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Domain.Models
{
    public class TeamMatchStats
    {
        public Guid Id { get; set; }

        public Guid MatchId { get; set; }

        public Match Match { get; set; } = null!;

        public Guid TeamId { get; set; }

        public Team Team { get; set; } = null!;

        public bool IsHome { get; set; }

        public string Formation { get; set; } = "";

        public string PlayingStyle { get; set; } = "";

        public MatchStats MatchStats { get; set; } = default!;
        public MatchShots MatchShots { get; set; } = default!;
        public MatchExpectedGoals MatchExpectedGoals { get; set; } = default!;
        public MatchPasses MatchPasses { get; set; } = default!;
        public MatchDiscipline MatchDiscipline { get; set; } = default!;
        public MatchDefence MatchDefence { get; set; } = default!;
        public MatchDuels MatchDuels { get; set; } = default!;
        public MatchAttackingZones MatchAttackingZones { get; set; } = default!;
        public MatchAnalysis MatchAnalysis { get; set; } = default!;
    }
}
