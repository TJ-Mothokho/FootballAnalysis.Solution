using FootballAnalysis.Data.Application.Common.ValueObjects;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.DTOs.TeamMatchStats
{
    public record GetTeamMatchStatsDTO(
        Guid Id,
        GetMatchDTO Match,
        GetTeamDTO Team,
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
