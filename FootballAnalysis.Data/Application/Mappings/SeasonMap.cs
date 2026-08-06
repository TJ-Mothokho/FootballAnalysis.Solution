using FootballAnalysis.Data.Application.DTOs.Season;
using FootballAnalysis.Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Mappings
{
    public static class SeasonMap
    {
        public static GetSeasonDTO ToGetDTO(Season season)
        {
            if (season == null) return null!;
            return new GetSeasonDTO(
                season.Id,
                season.Name,
                season.StartDate,
                season.EndDate,
                season.IsCurrent
            );
        }

        public static Season ToDomainModel(CreateSeasonDTO seasonDTO)
        {
            return new Season
            {
                Id = Guid.NewGuid(),
                Name = seasonDTO.Name,
                StartDate = seasonDTO.StartDate,
                EndDate = seasonDTO.EndDate,
                IsCurrent = seasonDTO.IsCurrent
            };
        }
    }
}
