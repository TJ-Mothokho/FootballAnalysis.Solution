using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.DTOs.Player
{
    public record UpdatePlayerDTO(
        string FirstName,
        string LastName,
        string Position,
        List<string>? AlternativePositions,
        int ShirtNumber,
        bool IsCaptain,
        bool IsActive,
        Guid? TeamId
    );
}
