using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.DTOs.Player
{
    public record CreatePlayerDTO(
        string FirstName,
        string LastName,
        string Position,
        List<string>? AlternativePositions,
        int ShirtNumber,
        DateOnly DateOfBirth,
        string National,
        bool IsActive,
        Guid? TeamId,
        bool? IsCaptain = false
    );
}
