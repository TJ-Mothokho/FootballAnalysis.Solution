using FootballAnalysis.Data.Application.DTOs.Season;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Application.LogExceptions;
using FootballAnalysis.Data.Application.Mappings;
using FootballAnalysis.Data.Application.Validations.Season;
using FootballAnalysis.Data.Application.Validations.Team;
using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Services
{
    public class SeasonService : IService<GetSeasonDTO, CreateSeasonDTO, UpdateSeasonDTO>
    {
        private readonly ISeasonRepository _seasonRepository;

        public SeasonService(ISeasonRepository seasonRepository)
        {
            _seasonRepository = seasonRepository;
        }

        public async Task<GetSeasonDTO> CreateAsync(CreateSeasonDTO entity)
        {
            try
            {
                var createValidator = new CreateSeasonValidator();
                var validatorResult = await createValidator.ValidateAsync(entity);

                if (!validatorResult.IsValid)
                {
                    LogException.LogExceptions(new ArgumentException("Invalid season data."));
                    throw new ArgumentException("Invalid season data.");
                }

                var season = SeasonMap.ToDomainModel(entity);
                var createdSeason = await _seasonRepository.AddAsync(season);
                return SeasonMap.ToGetDTO(createdSeason);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while creating a season.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _seasonRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while deleting a season.", ex);
            }
        }

        public async Task<IEnumerable<GetSeasonDTO>> GetAllAsync()
        {
            try
            {
                var seasons = await _seasonRepository.ListAsync();
                return seasons.Select(SeasonMap.ToGetDTO);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching seasons.", ex);
            }
        }

        public async Task<GetSeasonDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var season = await _seasonRepository.GetAsync(id);
                return SeasonMap.ToGetDTO(season);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching a season.", ex);
            }
        }

        public async Task<GetSeasonDTO> UpdateAsync(Guid id, UpdateSeasonDTO entity)
        {
            try
            {
                var updateValidator = new UpdateSeasonValidator();
                var validatorResult = await updateValidator.ValidateAsync(entity);

                if (!validatorResult.IsValid)
                {
                    LogException.LogExceptions(new ArgumentException("Invalid season data."));
                    throw new ArgumentException("Invalid season data.");
                }

                var existingSeason = await _seasonRepository.GetAsync(id);

                if (existingSeason == null)
                {
                    LogException.LogExceptions(new KeyNotFoundException("Season not found."));
                    throw new KeyNotFoundException("Season not found.");
                }

                existingSeason.StartDate = entity.StartDate;
                existingSeason.EndDate = entity.EndDate;
                existingSeason.IsCurrent = entity.IsCurrent;

                var updatedSeason = await _seasonRepository.UpdateAsync(existingSeason);
                return SeasonMap.ToGetDTO(updatedSeason);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while updating a season.", ex);
            }
        }
    }
}
