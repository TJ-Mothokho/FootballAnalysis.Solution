using FootballAnalysis.Data.Application.DTOs.PlayerMatchStats;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Application.LogExceptions;
using FootballAnalysis.Data.Application.Mappings;
using FootballAnalysis.Data.Application.Validations.PlayerMatchStats;
using FootballAnalysis.Data.Application.Validations.Team;
using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Services
{
    public class PlayerMatchStatsService : IPlayerMatchStatsService
    {
        private readonly IPlayerMatchStatsRepository _playerMatchStatsRepository;

        public PlayerMatchStatsService(IPlayerMatchStatsRepository playerMatchStatsRepository)
        {
            _playerMatchStatsRepository = playerMatchStatsRepository;
        }

        public async Task<GetPlayerMatchStatsDTO> CreateAsync(CreatePlayerMatchStatsDTO entity)
        {
            try
            {
                var createValidator = new CreatePlayerMatchStatsValidator();
                var validatorResult = await createValidator.ValidateAsync(entity);

                if (!validatorResult.IsValid)
                {
                    LogException.LogExceptions(new ArgumentException("Invalid player match stats data."));
                    throw new ArgumentException("Invalid player match stats data.");
                }

                var playerMatchStats = PlayerMatchStatsMap.ToDomainModel(entity);
                var createdPlayerMatchStats = await _playerMatchStatsRepository.AddAsync(playerMatchStats);
                return PlayerMatchStatsMap.ToGetDTO(createdPlayerMatchStats);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while creating player match stats.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _playerMatchStatsRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while deleting player match stats.", ex);
            }
        }

        public async Task<IEnumerable<GetPlayerMatchStatsDTO>> GetAllAsync()
        {
            try
            {
                var playerMatchStats = await _playerMatchStatsRepository.ListAsync();
                return playerMatchStats.Select(PlayerMatchStatsMap.ToGetDTO);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching player match stats.", ex);
            }
        }

        public async Task<GetPlayerMatchStatsDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var playerMatchStats = await _playerMatchStatsRepository.GetAsync(id);
                return PlayerMatchStatsMap.ToGetDTO(playerMatchStats);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching player match stats.", ex);
            }
        }

        public async Task<GetPlayerMatchStatsDTO> UpdateAsync(Guid id, UpdatePlayerMatchStatsDTO entity)
        {
            try
            {
                var updateValidator = new UpdatePlayerMatchStatsValidator();
                var validatorResult = await updateValidator.ValidateAsync(entity);

                if (!validatorResult.IsValid)
                {
                    LogException.LogExceptions(new ArgumentException("Invalid player match stats data."));
                    throw new ArgumentException("Invalid player match stats data.");
                }

                var existingPlayerMatchStats = await _playerMatchStatsRepository.GetAsync(id);

                if (existingPlayerMatchStats == null)
                {
                    LogException.LogExceptions(new KeyNotFoundException("Player match stats not found."));
                    throw new KeyNotFoundException("Player match stats not found.");
                }

                existingPlayerMatchStats.Started = entity.Started;
                existingPlayerMatchStats.WasSubstitutedOn = entity.WasSubstitutedOn;
                existingPlayerMatchStats.WasSubstitutedOff = entity.WasSubstitutedOff;
                existingPlayerMatchStats.IsCaptain = entity.IsCaptain;
                existingPlayerMatchStats.IsManOfTheMatch = entity.IsManOfTheMatch;
                existingPlayerMatchStats.Analysis = entity.Analysis ?? existingPlayerMatchStats.Analysis;
                existingPlayerMatchStats.PlayerStats = entity.PlayerStats;
                existingPlayerMatchStats.PlayerAttack = entity.PlayerAttack;
                existingPlayerMatchStats.PlayerPasses = entity.PlayerPasses;
                existingPlayerMatchStats.PlayerDefence = entity.PlayerDefence;
                existingPlayerMatchStats.PlayerDuels = entity.PlayerDuels;
                existingPlayerMatchStats.Goalkeepering = entity.Goalkeepering;
                existingPlayerMatchStats.PlayerDiscipline = entity.PlayerDiscipline;


                var updatedPlayerMatchStats = await _playerMatchStatsRepository.UpdateAsync(existingPlayerMatchStats);
                return PlayerMatchStatsMap.ToGetDTO(updatedPlayerMatchStats);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while updating player match stats.", ex);
            }
        }
    }
}
