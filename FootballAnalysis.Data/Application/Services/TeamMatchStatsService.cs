using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.DTOs.TeamMatchStats;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Application.LogExceptions;
using FootballAnalysis.Data.Application.Mappings;
using FootballAnalysis.Data.Application.Validations.Team;
using FootballAnalysis.Data.Application.Validations.TeamMatchStats;
using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Services
{
    public class TeamMatchStatsService : ITeamMatchStatsService
    {
        private readonly ITeamMatchStatsRepository _teamMatchStatsRepository;

        public TeamMatchStatsService(ITeamMatchStatsRepository teamMatchStatsRepository)
        {
            _teamMatchStatsRepository = teamMatchStatsRepository;
        }

        public async Task<GetTeamMatchStatsDTO> CreateAsync(CreateTeamMatchStatsDTO entity)
        {
            try
            {
                var createValidator = new CreateTeamMatchStatsValidator();
                var validatorResult = await createValidator.ValidateAsync(entity);

                if (!validatorResult.IsValid)
                {
                    LogException.LogExceptions(new ArgumentException("Invalid team match stats data."));
                    throw new ArgumentException("Invalid team match stats data.");
                }

                var teamMatchStats = TeamMatchStatsMap.ToDomainModel(entity);
                var createdTeamMatchStats = await _teamMatchStatsRepository.AddAsync(teamMatchStats);
                return TeamMatchStatsMap.ToGetDTO(createdTeamMatchStats);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while creating team match stats.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _teamMatchStatsRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while deleting team match stats.", ex);
            }
        }

        public async Task<IEnumerable<GetTeamMatchStatsDTO>> GetAllAsync()
        {
            try
            {
                var teamMatchStats = await _teamMatchStatsRepository.ListAsync();
                return teamMatchStats.Select(TeamMatchStatsMap.ToGetDTO);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching team match stats.", ex);
            }
        }

        public async Task<GetTeamMatchStatsDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var teamMatchStats = await _teamMatchStatsRepository.GetAsync(id);
                return TeamMatchStatsMap.ToGetDTO(teamMatchStats);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching team match stats.", ex);
            }
        }

        public async Task<GetTeamMatchStatsDTO> UpdateAsync(Guid id, UpdateTeamMatchStatsDTO entity)
        {
            try
            {
                var updateValidator = new UpdateTeamMatchStatsValidator();
                var validatorResult = await updateValidator.ValidateAsync(entity);

                if (!validatorResult.IsValid)
                {
                    LogException.LogExceptions(new ArgumentException("Invalid team match stats data."));
                    throw new ArgumentException("Invalid team match stats data.");
                }

                var existingTeamMatchStats = await _teamMatchStatsRepository.GetAsync(id);

                if (existingTeamMatchStats == null)
                {
                    LogException.LogExceptions(new KeyNotFoundException("Team match stats not found."));
                    throw new KeyNotFoundException("Team match stats not found.");
                }

                existingTeamMatchStats.IsHome = entity.IsHome;
                existingTeamMatchStats.Formation = entity.Formation ?? existingTeamMatchStats.Formation;
                existingTeamMatchStats.PlayingStyle = entity.PlayingStyle ?? existingTeamMatchStats.PlayingStyle;
                existingTeamMatchStats.MatchStats = entity.MatchStats ?? existingTeamMatchStats.MatchStats;
                existingTeamMatchStats.MatchShots = entity.MatchShots ?? existingTeamMatchStats.MatchShots;
                existingTeamMatchStats.MatchExpectedGoals = entity.MatchExpectedGoals ?? existingTeamMatchStats.MatchExpectedGoals;
                existingTeamMatchStats.MatchPasses = entity.MatchPasses ?? existingTeamMatchStats.MatchPasses;
                existingTeamMatchStats.MatchDiscipline = entity.MatchDiscipline ?? existingTeamMatchStats.MatchDiscipline;
                existingTeamMatchStats.MatchDefence = entity.MatchDefence ?? existingTeamMatchStats.MatchDefence;
                existingTeamMatchStats.MatchDuels = entity.MatchDuels ?? existingTeamMatchStats.MatchDuels;
                existingTeamMatchStats.MatchAttackingZones = entity.MatchAttackingZones ?? existingTeamMatchStats.MatchAttackingZones;
                existingTeamMatchStats.MatchAnalysis = entity.MatchAnalysis ?? existingTeamMatchStats.MatchAnalysis;


                var updatedTeamMatchStats = await _teamMatchStatsRepository.UpdateAsync(existingTeamMatchStats);
                return TeamMatchStatsMap.ToGetDTO(updatedTeamMatchStats);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while updating team match stats.", ex);
            }
        }
    }
}
