using FootballAnalysis.Data.Application.DTOs.Competition;
using FootballAnalysis.Data.Application.DTOs.Season;
using FootballAnalysis.Data.Application.DTOs.Team;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.DTOs.Match
{
    public record GetMatchDTO(
        Guid Id,
        DateTime KickOff,
        string Venue,
        string Referee,
        int? Attendance,
        int HomeGoals,
        int AwayGoals,
        string Status,
        GetTeamDTO HomeTeam,
        GetTeamDTO AwayTeam,
        GetCompetitionDTO Competition,
        GetSeasonDTO Season
    );
}
