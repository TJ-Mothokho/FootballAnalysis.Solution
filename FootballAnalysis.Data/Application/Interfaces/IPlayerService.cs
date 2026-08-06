using FootballAnalysis.Data.Application.DTOs.Player;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Application.Interfaces
{
    public interface IPlayerService : IService<GetPlayerDTO, CreatePlayerDTO, UpdatePlayerDTO>
    {
    }
}
