using FootballAnalysis.Data.Application.DTOs.Competition;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Season;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Mappings
{
    public static class MatchMap
    {
        public static GetMatchDTO ToGetDTO(Match match)
        {
            if (match == null) return null!;

            return new GetMatchDTO(
                match.Id,
                match.KickOff,
                match.Venue,
                match.Referee,
                match.Attendance,
                match.HomeGoals,
                match.AwayGoals,
                match.Status,
                match.HomeTeam != null ? TeamMap.ToGetDTO(match.HomeTeam) : null!,
                match.AwayTeam != null ? TeamMap.ToGetDTO(match.AwayTeam) : null!,
                match.Competition != null ? CompetitionMap.ToGetDTO(match.Competition) : null!,
                match.Season != null ? SeasonMap.ToGetDTO(match.Season) : null!
            );
        }

        public static Match ToDomainModel(CreateMatchDTO createMatchDTO)
        {
            if (createMatchDTO == null) return null!;
            return new Match
            {
                KickOff = createMatchDTO.KickOff,
                Venue = createMatchDTO.Venue,
                Referee = createMatchDTO.Referee,
                Attendance = createMatchDTO.Attendance,
                HomeGoals = createMatchDTO.HomeGoals,
                AwayGoals = createMatchDTO.AwayGoals,
                Status = createMatchDTO.Status,
                HomeTeamId = createMatchDTO.HomeTeamId,
                AwayTeamId = createMatchDTO.AwayTeamId,
                CompetitionId = createMatchDTO.CompetitionId,
                SeasonId = createMatchDTO.SeasonId
            };
        }
    }
}
