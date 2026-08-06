using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.DTOs.Match
{
    public record CreateMatchDTO(
       DateTime KickOff,
       string Venue,
       string Referee,
       int? Attendance,
       int HomeGoals,
       int AwayGoals,
       string Status,
       Guid HomeTeamId,
       Guid AwayTeamId,
       Guid CompetitionId,
       Guid SeasonId
   );
}
