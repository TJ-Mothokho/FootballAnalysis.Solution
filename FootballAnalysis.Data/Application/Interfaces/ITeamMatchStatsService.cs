using FootballAnalysis.Data.Application.DTOs.TeamMatchStats;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface ITeamMatchStatsService : IService<GetTeamMatchStatsDTO, CreateTeamMatchStatsDTO, UpdateTeamMatchStatsDTO>
    {
    }
}
