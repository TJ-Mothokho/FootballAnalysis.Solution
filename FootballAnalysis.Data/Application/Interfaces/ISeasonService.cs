using FootballAnalysis.Data.Application.DTOs.Season;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface ISeasonService : IService<GetSeasonDTO, CreateSeasonDTO, UpdateSeasonDTO>
    {
    }
}
