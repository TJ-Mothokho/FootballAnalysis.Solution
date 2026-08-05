using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.DTOs.Team
{
    public record CreateTeamDTO(
        string Name,
        string ShortName,
        string Stadium,
        string City,
        string Coach,
        Guid? Captain,
        string? PreferredFormation,
        string? PlayingStyle
    );
}
