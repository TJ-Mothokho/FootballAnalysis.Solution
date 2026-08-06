using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Application.LogExceptions;
using FootballAnalysis.Data.Application.Mappings;
using FootballAnalysis.Data.Application.Validations.Team;
using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;

        public TeamService(ITeamRepository teamRepository, IPlayerRepository playerRepository)
        {
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
        }

        public async Task<GetTeamDTO> CreateAsync(CreateTeamDTO entity)
        {
            try
            {
                var createValidator = new CreateTeamValidator();
                var validatorResult = await createValidator.ValidateAsync(entity);

                if (!validatorResult.IsValid)
                {
                    LogException.LogExceptions(new ArgumentException("Invalid team data."));
                    throw new ArgumentException("Invalid team data.");
                }

                var team = TeamMap.ToDomainModel(entity);
                var createdTeam = await _teamRepository.AddAsync(team);
                return TeamMap.ToGetDTO(createdTeam);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while creating a team.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _teamRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while deleting a team.", ex);
            }
        }

        public async Task<IEnumerable<GetTeamDTO>> GetAllAsync()
        {
            try
            {
                var teams = await _teamRepository.ListAsync();
                return teams.Select(TeamMap.ToGetDTO);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching teams.", ex);
            }
        }

        public async Task<GetTeamDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var team = await _teamRepository.GetAsync(id);
                return TeamMap.ToGetDTO(team);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching a team.", ex);
            }
        }

        public async Task<GetTeamDTO> UpdateAsync(Guid id, UpdateTeamDTO entity)
        {
            try
            {
                var updateValidator = new UpdateTeamValidator();
                var validatorResult = await updateValidator.ValidateAsync(entity);

                if (!validatorResult.IsValid)
                {
                    LogException.LogExceptions(new ArgumentException("Invalid team data."));
                    throw new ArgumentException("Invalid team data.");
                }

                var existingTeam = await _teamRepository.GetAsync(id);

                if (existingTeam == null)
                {
                    LogException.LogExceptions(new KeyNotFoundException("Team not found."));
                    throw new KeyNotFoundException("Team not found.");
                }

                if(entity.FoundedYear < 0)
                {
                    LogException.LogExceptions(new ArgumentException("Founded year cannot be negative."));
                    throw new ArgumentException("Founded year cannot be negative.");
                }

                if(entity.Captain != null)
                {
                    existingTeam.Captain = await _playerRepository.GetAsync(entity.Captain);
                }

                existingTeam.Name = entity.Name ?? existingTeam.Name;
                existingTeam.ShortName = entity.ShortName ?? existingTeam.ShortName;
                existingTeam.Stadium = entity.Stadium ?? existingTeam.Stadium;
                existingTeam.City = entity.City ?? existingTeam.City;
                existingTeam.FoundedYear = entity.FoundedYear > 0 ? entity.FoundedYear : existingTeam.FoundedYear;
                existingTeam.Coach = entity.Coach ?? existingTeam.Coach;
                existingTeam.PreferredFormation = entity.PreferredFormation ?? existingTeam.PreferredFormation;
                existingTeam.PlayingStyle = entity.PlayingStyle ?? existingTeam.PlayingStyle;


                var updatedTeam = await _teamRepository.UpdateAsync(existingTeam);
                return TeamMap.ToGetDTO(updatedTeam);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while updating a team.", ex);
            }
        }
    }
}
