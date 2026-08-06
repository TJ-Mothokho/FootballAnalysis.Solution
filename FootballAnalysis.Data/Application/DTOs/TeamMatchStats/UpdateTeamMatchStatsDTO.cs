
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.DTOs.TeamMatchStats
{
    public record UpdateTeamMatchStatsDTO(
        bool IsHome,
        string Formation,
        string PlayingStyle,
        MatchStats MatchStats,
        MatchShots MatchShots,
        MatchExpectedGoals MatchExpectedGoals,
        MatchPasses MatchPasses,
        MatchDiscipline MatchDiscipline,
        MatchDefence MatchDefence,
        MatchDuels MatchDuels,
        MatchAttackingZones MatchAttackingZones,
        MatchAnalysis? MatchAnalysis
    );
}
