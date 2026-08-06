using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Mappings
{
    public static class TeamMap
    {
        public static GetTeamDTO ToGetDTO(Team team)
        {
            if (team == null) return null!;

            return new GetTeamDTO(
                team.Id,
                team.Name,
                team.ShortName,
                team.Stadium,
                team.City,
                team.FoundedYear,
                team.Coach,
                team.Captain != null ? PlayerMap.ToGetDTO(team.Captain) : null,
                team.PreferredFormation,
                team.PlayingStyle
            );
        }

        public static Team ToDomainModel(CreateTeamDTO teamDTO)
        {
            return new Team
            {
                Id = Guid.NewGuid(),
                Name = teamDTO.Name,
                ShortName = teamDTO.ShortName,
                Stadium = teamDTO.Stadium,
                City = teamDTO.City,
                FoundedYear = teamDTO.FoundedYear,
                Coach = teamDTO.Coach,
                PreferredFormation = teamDTO.PreferredFormation,
                PlayingStyle = teamDTO.PlayingStyle
            };
        }
    }
}
