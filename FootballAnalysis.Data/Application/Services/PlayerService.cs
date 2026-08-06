using FootballAnalysis.Data.Application.DTOs.Player;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Application.LogExceptions;
using FootballAnalysis.Data.Application.Mappings;
using FootballAnalysis.Data.Application.Validations.Player;
using FootballAnalysis.Data.Application.Validations.Team;
using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Services
{
    public class PlayerService : IService<GetPlayerDTO, CreatePlayerDTO, UpdatePlayerDTO>
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public async Task<GetPlayerDTO> CreateAsync(CreatePlayerDTO entity)
        {
            try
            {
                var createValidator = new CreatePlayerValidator();
                var validatorResult = await createValidator.ValidateAsync(entity);

                if (!validatorResult.IsValid)
                {
                    LogException.LogExceptions(new ArgumentException("Invalid player data."));
                    throw new ArgumentException("Invalid player data.");
                }

                var player = PlayerMap.ToDomainModel(entity);
                var createdPlayer = await _playerRepository.AddAsync(player);
                return PlayerMap.ToGetDTO(createdPlayer);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while creating a player.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _playerRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while deleting a player.", ex);
            }
        }

        public async Task<IEnumerable<GetPlayerDTO>> GetAllAsync()
        {
            try
            {
                var players = await _playerRepository.ListAsync();
                return players.Select(PlayerMap.ToGetDTO);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching players.", ex);
            }
        }

        public async Task<GetPlayerDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var player = await _playerRepository.GetAsync(id);
                return PlayerMap.ToGetDTO(player);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching a player.", ex);
            }
        }

        public async Task<GetPlayerDTO> UpdateAsync(Guid id, UpdatePlayerDTO entity)
        {
            try
            {
                var updateValidator = new UpdatePlayerValidator();
                var validatorResult = await updateValidator.ValidateAsync(entity);

                if (!validatorResult.IsValid)
                {
                    LogException.LogExceptions(new ArgumentException("Invalid player data."));
                    throw new ArgumentException("Invalid player data.");
                }

                var existingPlayer = await _playerRepository.GetAsync(id);

                if (existingPlayer == null)
                {
                    LogException.LogExceptions(new KeyNotFoundException("Player not found."));
                    throw new KeyNotFoundException("Player not found.");
                }

                existingPlayer.FirstName = entity.FirstName ?? existingPlayer.FirstName;
                existingPlayer.LastName = entity.LastName ?? existingPlayer.LastName;
                existingPlayer.Position = entity.Position ?? existingPlayer.Position;
                existingPlayer.AlternativePositions = entity.AlternativePositions ?? existingPlayer.AlternativePositions;
                existingPlayer.ShirtNumber = entity.ShirtNumber != 0 ? entity.ShirtNumber : existingPlayer.ShirtNumber;
                existingPlayer.IsCaptain = entity.IsCaptain;
                existingPlayer.IsActive = entity.IsActive;
                
                if(existingPlayer.TeamId != entity.TeamId && entity.TeamId != Guid.Empty || entity.TeamId != null)
                {
                    existingPlayer.TeamId = entity.TeamId!.Value;
                }

                var updatedPlayer = await _playerRepository.UpdateAsync(existingPlayer);
                return PlayerMap.ToGetDTO(updatedPlayer);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while updating a player.", ex);
            }
        }
    }
}
