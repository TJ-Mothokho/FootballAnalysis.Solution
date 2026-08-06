using FootballAnalysis.Data.Application.DTOs.TeamMatchStats;
using FootballAnalysis.Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Mappings
{
    public static class TeamMatchStatsMap
    {
        public static GetTeamMatchStatsDTO ToGetDTO(TeamMatchStats teamMatchStats)
        {
            if (teamMatchStats == null) return null!;

            return new GetTeamMatchStatsDTO(
                teamMatchStats.Id,
                teamMatchStats.Match != null ? MatchMap.ToGetDTO(teamMatchStats.Match) : null,
                teamMatchStats.Team != null ? TeamMap.ToGetDTO(teamMatchStats.Team) : null,
                teamMatchStats.IsHome,
                teamMatchStats.Formation,
                teamMatchStats.PlayingStyle,
                teamMatchStats.MatchStats,
                teamMatchStats.MatchShots,
                teamMatchStats.MatchExpectedGoals,
                teamMatchStats.MatchPasses,
                teamMatchStats.MatchDiscipline,
                teamMatchStats.MatchDefence,
                teamMatchStats.MatchDuels,
                teamMatchStats.MatchAttackingZones,
                teamMatchStats.MatchAnalysis
            );
        }

        public static TeamMatchStats ToDomainModel(CreateTeamMatchStatsDTO teamMatchStatsDTO)
        {
            return new TeamMatchStats
            {
                Id = Guid.NewGuid(),
                MatchId = teamMatchStatsDTO.MatchId,
                TeamId = teamMatchStatsDTO.TeamId,
                IsHome = teamMatchStatsDTO.IsHome,
                Formation = teamMatchStatsDTO.Formation,
                PlayingStyle = teamMatchStatsDTO.PlayingStyle,
                MatchStats = teamMatchStatsDTO.MatchStats,
                MatchShots = teamMatchStatsDTO.MatchShots,
                MatchExpectedGoals = teamMatchStatsDTO.MatchExpectedGoals,
                MatchPasses = teamMatchStatsDTO.MatchPasses,
                MatchDiscipline = teamMatchStatsDTO.MatchDiscipline,
                MatchDefence = teamMatchStatsDTO.MatchDefence,
                MatchDuels = teamMatchStatsDTO.MatchDuels,
                MatchAttackingZones = teamMatchStatsDTO.MatchAttackingZones,
                MatchAnalysis = teamMatchStatsDTO.MatchAnalysis
            };
        }
    }
}
