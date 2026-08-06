using FootballAnalysis.Data.Application.DTOs.Competition;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Application.LogExceptions;
using FootballAnalysis.Data.Application.Mappings;
using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Domain.Models;
using FootballAnalysis.Data.Application.Validations.Competition;
using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace FootballAnalysis.Data.Application.Services
{
    public class CompetitionService : ICompetitionService
    {
        private readonly ICompetitionRepository _competitionRepository;

        public CompetitionService(ICompetitionRepository competitionRepository)
        {
            _competitionRepository = competitionRepository;
        }

        public async Task<GetCompetitionDTO> CreateAsync(CreateCompetitionDTO competitionDto)
        {
            try
            {
                // Validate the input DTO
                var createValidator = new CreateCompetitionValidator();
                var validationResult = await createValidator.ValidateAsync(competitionDto);

                if (!validationResult.IsValid)
                {
                    LogException.LogExceptions(new ValidationException("Invalid competition data.", validationResult.Errors));
                    throw new ValidationException("Invalid competition data.", validationResult.Errors);
                }

                // Create a new Competition entity from the DTO
                var competition = new Competition
                {
                    Id = Guid.NewGuid(),
                    Name = competitionDto.Name,
                    Country = competitionDto.Country
                };

                // Save the new competition to the repository
                var createdCompetition = await _competitionRepository.AddAsync(competition);

                var _competition = CompetitionMap.ToGetDTO(createdCompetition);

                return _competition;
            }
            catch (Exception ex)
            {
                // Log the exception
                LogException.LogExceptions(ex);

                // Handle it as needed
                throw new ApplicationException("An error occurred while adding competition.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                // Find the competition by ID
                var competition = await _competitionRepository.GetAsync(id);

                if(competition == null)
                    throw new KeyNotFoundException($"Competition with ID {id} not found.");

                await _competitionRepository.DeleteAsync(competition);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                LogException.LogExceptions(ex);

                // Handle it as needed
                throw new ApplicationException("An error occurred while deleting competition.", ex);
            }
        }

        public async Task<IEnumerable<GetCompetitionDTO>> GetAllAsync()
        {
            try
            {
                // Retrieve all competitions from the repository
                var competitions = await _competitionRepository.ListAsync();
                return competitions.Select(CompetitionMap.ToGetDTO);
            }
            catch (Exception ex)
            {
                // Log the exception
                LogException.LogExceptions(ex);                
                
                // Handle it as needed
                throw new ApplicationException("An error occurred while retrieving competitions.", ex);
            }
        }

        public async Task<GetCompetitionDTO> GetByIdAsync(Guid id)
        {
            try
            {
                // Retrieve the competition by ID from the repository
                var competition = await _competitionRepository.GetAsync(id);

                if (competition == null)
                {
                    LogException.LogExceptions(new KeyNotFoundException($"Competition with ID {id} not found."));
                    throw new KeyNotFoundException($"Competition with ID {id} not found.");
                }

                return CompetitionMap.ToGetDTO(competition);
            }
            catch (Exception ex)
            {
                // Log the exception
                LogException.LogExceptions(ex);

                // Handle it as needed
                throw new ApplicationException("An error occurred while retrieving competition.", ex);
            }
        }

        public async Task<GetCompetitionDTO> UpdateAsync(Guid id, UpdateCompetitionDTO competitionDto)
        {
            try
            {
                // Validate the input DTO
                var updateValidator = new UpdateCompetitionValidator();
                var validationResult = await updateValidator.ValidateAsync(competitionDto);

                if (!validationResult.IsValid)
                {
                    LogException.LogExceptions(new ValidationException("Invalid competition data.", validationResult.Errors));
                    throw new ValidationException("Invalid competition data.", validationResult.Errors);
                }

                // Find the existing competition by ID
                var existingCompetition = await _competitionRepository.GetAsync(id);

                // If the competition doesn't exist, throw an exception
                if (existingCompetition == null)
                {
                    LogException.LogExceptions(new KeyNotFoundException($"Competition with ID {id} not found."));
                    throw new KeyNotFoundException($"Competition with ID {id} not found.");
                }

                // Update the competition's properties
                existingCompetition.Name = competitionDto.Name ?? existingCompetition.Name;
                existingCompetition.Country = competitionDto.Country ?? existingCompetition.Country;

                // Save the updated competition to the repository
                var updatedCompetition = await _competitionRepository.UpdateAsync(existingCompetition);

                var _competition = CompetitionMap.ToGetDTO(updatedCompetition);

                return _competition;
            }
            catch (Exception ex)
            {
                // Log the exception
                LogException.LogExceptions(ex);

                // Handle it as needed
                throw new ApplicationException("An error occurred while updating competition.", ex);
            }
        }
    }
}
