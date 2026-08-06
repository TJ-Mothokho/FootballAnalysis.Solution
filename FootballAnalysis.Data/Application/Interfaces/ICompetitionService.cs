using FootballAnalysis.Data.Application.DTOs.Competition;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface ICompetitionService : IService<GetCompetitionDTO, CreateCompetitionDTO, UpdateCompetitionDTO>
    {

        //Task<IEnumerable<GetCompetitionDTO>> GetAllCompetitionsAsync();
        //Task<GetCompetitionDTO> GetCompetitionByIdAsync(Guid id);
        //Task<GetCompetitionDTO> CreateCompetitionAsync(CreateCompetitionDTO competitionDto);
        //Task<GetCompetitionDTO> UpdateCompetitionAsync(Guid id, UpdateCompetitionDTO competitionDto);
        //Task<bool> DeleteCompetitionAsync(Guid id);
    }
}
