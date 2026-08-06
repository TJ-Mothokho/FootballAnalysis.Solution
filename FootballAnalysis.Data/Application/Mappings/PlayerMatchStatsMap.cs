using FootballAnalysis.Data.Application.DTOs.PlayerMatchStats;
using FootballAnalysis.Data.Domain.Models;

namespace FootballAnalysis.Data.Application.Mappings
{
    public static class PlayerMatchStatsMap
    {
        public static GetPlayerMatchStatsDTO ToGetDTO(PlayerMatchStats playerMatchStats)
        {
            if (playerMatchStats == null) return null!;
            return new GetPlayerMatchStatsDTO(
                playerMatchStats.Id,
                playerMatchStats.Match != null ? MatchMap.ToGetDTO(playerMatchStats.Match) : null!,
                playerMatchStats.Player != null ? PlayerMap.ToGetDTO(playerMatchStats.Player) : null!,
                playerMatchStats.Team != null ? TeamMap.ToGetDTO(playerMatchStats.Team) : null!,
                playerMatchStats.Started,
                playerMatchStats.WasSubstitutedOn,
                playerMatchStats.WasSubstitutedOff,
                playerMatchStats.IsCaptain,
                playerMatchStats.IsManOfTheMatch,
                playerMatchStats.Analysis,
                playerMatchStats.PlayerStats,
                playerMatchStats.PlayerAttack,
                playerMatchStats.PlayerPasses,
                playerMatchStats.PlayerDefence,
                playerMatchStats.PlayerDuels,
                playerMatchStats.Goalkeepering,
                playerMatchStats.PlayerDiscipline
            );
        }
        public static PlayerMatchStats ToDomainModel(CreatePlayerMatchStatsDTO playerMatchStatsDTO)
        {
            return new PlayerMatchStats
            {
                MatchId = playerMatchStatsDTO.MatchId,
                PlayerId = playerMatchStatsDTO.PlayerId,
                TeamId = playerMatchStatsDTO.TeamId,
                Started = playerMatchStatsDTO.Started,
                WasSubstitutedOn = playerMatchStatsDTO.WasSubstitutedOn,
                WasSubstitutedOff = playerMatchStatsDTO.WasSubstitutedOff,
                IsCaptain = playerMatchStatsDTO.IsCaptain,
                Analysis = playerMatchStatsDTO.Analysis,
                PlayerStats = playerMatchStatsDTO.PlayerStats,
                PlayerAttack = playerMatchStatsDTO.PlayerAttack,
                PlayerPasses = playerMatchStatsDTO.PlayerPasses,
                PlayerDefence = playerMatchStatsDTO.PlayerDefence,
                PlayerDuels = playerMatchStatsDTO.PlayerDuels,
                Goalkeepering = playerMatchStatsDTO.Goalkeepering,
                PlayerDiscipline = playerMatchStatsDTO.PlayerDiscipline,
                IsManOfTheMatch = playerMatchStatsDTO.IsManOfTheMatch.Value
            };
        }
    }
}
