using FootballAnalysis.Data.Application.DTOs.Match;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface IMatchService : IService<GetMatchDTO, CreateMatchDTO, UpdateMatchDTO>
    {
    }
}
