
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Player;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.DTOs.PlayerMatchStats
{
    public record UpdatePlayerMatchStatsDTO(
        bool Started,
        bool WasSubstitutedOn,
        bool WasSubstitutedOff,
        bool IsCaptain,
        bool IsManOfTheMatch,
        PlayerAnalysis? Analysis,
        PlayerStats PlayerStats,
        PlayerAttack PlayerAttack,
        PlayerPasses PlayerPasses,
        PlayerDefence PlayerDefence,
        PlayerDuels PlayerDuels,
        Goalkeepering Goalkeepering,
        PlayerDiscipline PlayerDiscipline
    );
}
