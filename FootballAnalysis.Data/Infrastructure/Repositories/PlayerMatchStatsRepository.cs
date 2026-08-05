using FootballAnalysis.Data.Domain.Interfaces;
using FootballAnalysis.Data.Domain.Models;
using FootballAnalysis.Data.Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace FootballAnalysis.Data.Infrastructure.Repositories
{
    public class PlayerMatchStatsRepository : Repository<PlayerMatchStats>, IPlayerMatchStatsRepository
    {
        private readonly ApplicationDbContext _context;

        public PlayerMatchStatsRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
