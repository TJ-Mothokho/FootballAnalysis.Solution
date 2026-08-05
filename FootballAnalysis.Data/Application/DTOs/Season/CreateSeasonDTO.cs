using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.DTOs.Season
{
    public record CreateSeasonDTO(
        string Name,
        DateOnly StartDate,
        DateOnly EndDate,
        bool IsCurrent
    );
}
