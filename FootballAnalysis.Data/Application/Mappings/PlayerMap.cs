using FootballAnalysis.Data.Application.DTOs.Player;
using FootballAnalysis.Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Mappings
{
    public static class PlayerMap
    {
        public static GetPlayerDTO ToGetDTO(Player player)
        {
            if (player == null) return null!;
            return new GetPlayerDTO(
                player.Id,
                player.FirstName,
                player.LastName,
                player.Position,
                player.AlternativePositions,
                player.ShirtNumber,
                player.DateOfBirth.Value,
                player.Nationality,
                player.IsCaptain,
                player.IsActive,
                player.Team != null ? TeamMap.ToGetDTO(player.Team) : null
            );
        }

        public static Player ToDomainModel(CreatePlayerDTO playerDTO)
        {
            return new Player
            {
                Id = Guid.NewGuid(),
                FirstName = playerDTO.FirstName,
                LastName = playerDTO.LastName,
                Position = playerDTO.Position,
                AlternativePositions = playerDTO.AlternativePositions,
                ShirtNumber = playerDTO.ShirtNumber,
                DateOfBirth = playerDTO.DateOfBirth,
                Nationality = playerDTO.National,
                IsCaptain = playerDTO.IsCaptain??false,
                IsActive = playerDTO.IsActive,
                TeamId = playerDTO.TeamId ?? Guid.Empty
            };
        }
    }
}
