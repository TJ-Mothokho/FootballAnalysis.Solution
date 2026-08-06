using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.Interfaces;
using FootballAnalysis.Data.Application.LogExceptions;
using FootballAnalysis.Data.Application.Mappings;
using FootballAnalysis.Data.Application.Validations.Match;
using FootballAnalysis.Data.Application.Validations.Team;
using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Services
{
    public class MatchService : IService<GetMatchDTO, CreateMatchDTO, UpdateMatchDTO>
    {
        private readonly IMatchRepository _matchRepository;

        public MatchService(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        public async Task<GetMatchDTO> CreateAsync(CreateMatchDTO entity)
        {
            try
            {
                var createValidator = new CreateMatchValidator();
                var validatorResult = await createValidator.ValidateAsync(entity);

                if (!validatorResult.IsValid)
                {
                    LogException.LogExceptions(new ArgumentException("Invalid match data."));
                    throw new ArgumentException("Invalid match data.");
                }

                var match = MatchMap.ToDomainModel(entity);
                var createdMatch = await _matchRepository.AddAsync(match);
                return MatchMap.ToGetDTO(createdMatch);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while creating a match.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _matchRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while deleting a match.", ex);
            }
        }

        public async Task<IEnumerable<GetMatchDTO>> GetAllAsync()
        {
            try
            {
                var matches = await _matchRepository.ListAsync();
                return matches.Select(MatchMap.ToGetDTO);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching matches.", ex);
            }
        }

        public async Task<GetMatchDTO> GetByIdAsync(Guid id)
        {
            try
            {
                var match = await _matchRepository.GetAsync(id);
                return MatchMap.ToGetDTO(match);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while fetching a match.", ex);
            }
        }

        public async Task<GetMatchDTO> UpdateAsync(Guid id, UpdateMatchDTO entity)
        {
            try
            {
                var updateValidator = new UpdateMatchValidator();
                var validatorResult = await updateValidator.ValidateAsync(entity);

                if (!validatorResult.IsValid)
                {
                    LogException.LogExceptions(new ArgumentException("Invalid match data."));
                    throw new ArgumentException("Invalid match data.");
                }

                var existingMatch = await _matchRepository.GetAsync(id);

                if (existingMatch == null)
                {
                    LogException.LogExceptions(new KeyNotFoundException("Match not found."));
                    throw new KeyNotFoundException("Match not found.");
                }

                existingMatch.KickOff = entity.KickOff;
                existingMatch.Venue = entity.Venue ?? existingMatch.Venue;
                existingMatch.Status = entity.Status ?? existingMatch.Status;
                existingMatch.Attendance = entity.Attendance ?? existingMatch.Attendance;
                existingMatch.Referee = entity.Referee ?? existingMatch.Referee;
                existingMatch.HomeGoals = entity.HomeGoals != existingMatch.HomeGoals ? entity.HomeGoals : existingMatch.HomeGoals;
                existingMatch.AwayGoals = entity.AwayGoals != existingMatch.AwayGoals ? entity.AwayGoals : existingMatch.AwayGoals;
                existingMatch.Status = entity.Status ?? existingMatch.Status;

                var updatedMatch = await _matchRepository.UpdateAsync(existingMatch);
                return MatchMap.ToGetDTO(updatedMatch);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error occurred while updating a match.", ex);
            }
        }
    }
}
