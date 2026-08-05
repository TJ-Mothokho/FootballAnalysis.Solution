using FootballAnalysis.Data.Application.DTOs.Competition;
using FootballAnalysis.Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Mappings
{
    public static class CompetitionMap
    {
        public static Competition ToDomainModel(CreateCompetitionDTO competitionDTO)
        {
            return new Competition
            {
                Id = Guid.NewGuid(),
                Name = competitionDTO.Name,
                Country = competitionDTO.Country
            };
        }

        public static GetCompetitionDTO ToGetDTO(Competition competition)
        {
            return new GetCompetitionDTO(
                competition.Id,
                competition.Name,
                competition.Country,
                null,
                null
            );
        }

    }


}
