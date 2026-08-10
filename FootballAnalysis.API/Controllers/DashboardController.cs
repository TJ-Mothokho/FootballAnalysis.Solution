using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballAnalysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("overview")]
        public async Task<ActionResult<DashboardOverviewDTO>> GetOverview() => Ok(await _dashboardService.GetOverviewAsync());

        [HttpGet("top-scorers")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetTopScorers() => Ok(await _dashboardService.GetTopScorersAsync());

        [HttpGet("top-assists")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetTopAssists() => Ok(await _dashboardService.GetTopAssistsAsync());

        [HttpGet("top-rated")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetTopRated() => Ok(await _dashboardService.GetTopRatedAsync());

        [HttpGet("form-table")]
        public async Task<ActionResult<IEnumerable<StandingDTO>>> GetFormTable() => Ok(await _dashboardService.GetFormTableAsync());

        [HttpGet("recent-matches")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetRecentMatches() => Ok(await _dashboardService.GetRecentMatchesAsync());

        [HttpGet("upcoming-fixtures")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetUpcomingFixtures() => Ok(await _dashboardService.GetUpcomingFixturesAsync());

        [HttpGet("goals-per-round")]
        public async Task<ActionResult<IEnumerable<GoalsPerRoundDTO>>> GetGoalsPerRound() => Ok(await _dashboardService.GetGoalsPerRoundAsync());
    }
}
