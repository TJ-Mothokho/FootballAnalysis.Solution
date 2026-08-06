
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Player;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.DTOs.PlayerMatchStats
{
    public record CreatePlayerMatchStatsDTO(
        Guid MatchId,
        Guid PlayerId,
        Guid TeamId,
        bool Started,
        bool WasSubstitutedOn,
        bool WasSubstitutedOff,
        bool IsCaptain,
        PlayerAnalysis? Analysis,
        PlayerStats PlayerStats,
        PlayerAttack PlayerAttack,
        PlayerPasses PlayerPasses,
        PlayerDefence PlayerDefence,
        PlayerDuels PlayerDuels,
        Goalkeepering Goalkeepering,
        PlayerDiscipline PlayerDiscipline,
        bool? IsManOfTheMatch = false
    );
}
