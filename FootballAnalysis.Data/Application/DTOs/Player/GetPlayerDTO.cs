using FootballAnalysis.Data.Application.DTOs.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.DTOs.Player
{
    public record GetPlayerDTO(
        Guid Id,
        string FirstName,
        string LastName,
        string Position,
        List<string>? AlternativePositions,
        int ShirtNumber,
        DateOnly DateOfBirth,
        string National,
        bool IsCaptain,
        bool IsActive,
        GetTeamDTO? Team
    );
}
