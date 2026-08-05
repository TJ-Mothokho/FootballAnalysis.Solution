using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Season;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.DTOs.Competition
{
    public record GetCompetitionDTO(
        Guid Id,
        string Name,
        string Country,
        List<GetSeasonDTO>? Seasons,
        List<GetMatchDTO>? Matches);
}
