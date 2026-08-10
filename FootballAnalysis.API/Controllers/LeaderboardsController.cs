using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballAnalysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaderboardsController : ControllerBase
    {
        private readonly ILeaderboardService _leaderboardService;

        public LeaderboardsController(ILeaderboardService leaderboardService)
        {
            _leaderboardService = leaderboardService;
        }

        [HttpGet("goals")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetGoals() => Ok(await _leaderboardService.GetGoalsAsync());

        [HttpGet("assists")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetAssists() => Ok(await _leaderboardService.GetAssistsAsync());

        [HttpGet("ratings")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetRatings() => Ok(await _leaderboardService.GetRatingsAsync());

        [HttpGet("passes")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetPasses() => Ok(await _leaderboardService.GetPassesAsync());

        [HttpGet("chances-created")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetChancesCreated() => Ok(await _leaderboardService.GetChancesCreatedAsync());

        [HttpGet("tackles")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetTackles() => Ok(await _leaderboardService.GetTacklesAsync());

        [HttpGet("interceptions")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetInterceptions() => Ok(await _leaderboardService.GetInterceptionsAsync());

        [HttpGet("clean-sheets")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetCleanSheets() => Ok(await _leaderboardService.GetCleanSheetsAsync());

        [HttpGet("saves")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetSaves() => Ok(await _leaderboardService.GetSavesAsync());
    }
}
