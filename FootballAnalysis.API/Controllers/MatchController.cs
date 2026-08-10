using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.PlayerMatchStats;
using FootballAnalysis.Data.Application.DTOs.TeamMatchStats;
using FootballAnalysis.Data.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballAnalysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchService _matchService;

        public MatchController(IMatchService matchService)
        {
            _matchService = matchService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetMatches() => Ok(await _matchService.GetAllAsync());

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<GetMatchDTO>> GetMatch(Guid id) => Ok(await _matchService.GetByIdAsync(id));

        [HttpPost("create")]
        public async Task<ActionResult<GetMatchDTO>> CreateMatch(CreateMatchDTO matchDto)
        {
            var createdMatch = await _matchService.CreateAsync(matchDto);
            return CreatedAtAction(nameof(GetMatch), new { id = createdMatch.Id }, createdMatch);
        }

        [HttpPost("delete")]
        public async Task<ActionResult> DeleteMatch(Guid id)
        {
            await _matchService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("update")]
        public async Task<ActionResult<GetMatchDTO>> UpdateMatch(Guid id, UpdateMatchDTO matchDto) => Ok(await _matchService.UpdateAsync(id, matchDto));

        [HttpGet("upcoming")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetUpcoming() => Ok(await _matchService.GetUpcomingAsync());

        [HttpGet("completed")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetCompleted() => Ok(await _matchService.GetCompletedAsync());

        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetLatest() => Ok(await _matchService.GetLatestAsync());

        [HttpGet("today")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetToday() => Ok(await _matchService.GetTodayAsync());

        [HttpGet("by-team/{teamId:Guid}")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetByTeam(Guid teamId) => Ok(await _matchService.GetByTeamAsync(teamId));

        [HttpGet("by-competition/{competitionId:Guid}")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetByCompetition(Guid competitionId) => Ok(await _matchService.GetByCompetitionAsync(competitionId));

        [HttpGet("by-season/{seasonId:Guid}")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetBySeason(Guid seasonId) => Ok(await _matchService.GetBySeasonAsync(seasonId));

        [HttpGet("{id:Guid}/team-stats")]
        public async Task<ActionResult<IEnumerable<GetTeamMatchStatsDTO>>> GetTeamStats(Guid id) => Ok(await _matchService.GetTeamStatsAsync(id));

        [HttpGet("{id:Guid}/player-stats")]
        public async Task<ActionResult<IEnumerable<GetPlayerMatchStatsDTO>>> GetPlayerStats(Guid id) => Ok(await _matchService.GetPlayerStatsAsync(id));

        [HttpGet("{id:Guid}/summary")]
        public async Task<ActionResult<MatchSummaryDTO>> GetSummary(Guid id) => Ok(await _matchService.GetSummaryAsync(id));

        [HttpGet("{id:Guid}/workspace")]
        public async Task<ActionResult<MatchWorkspaceDTO>> GetWorkspace(Guid id) => Ok(await _matchService.GetWorkspaceAsync(id));
    }
}
