using FootballAnalysis.Data.Application.DTOs.Player;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.DTOs.Team
{
    public record GetTeamDTO(
        Guid Id,
        string Name,
        string ShortName,
        string Stadium,
        string City,
        int FoundedYear,
        string Coach,
        GetPlayerDTO? Captain,
        string? PreferredFormation,
        string? PlayingStyle
    );
}
