using FootballAnalysis.Data.Application.DTOs.PlayerMatchStats;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface IPlayerMatchStatsService : IService<GetPlayerMatchStatsDTO, CreatePlayerMatchStatsDTO, UpdatePlayerMatchStatsDTO>
    {
    }
}
