using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.DTOs.Team
{
    public record UpdateTeamDTO(
        string Name,
        string ShortName,
        string Stadium,
        string City,
        int FoundedYear,
        string Coach,
        Guid? Captain,
        string? PreferredFormation,
        string? PlayingStyle
    );
}
