using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Player;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballAnalysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GetTeamDTO>>> GetTeams() => Ok(await _teamService.GetAllAsync());

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<GetTeamDTO>> GetTeam(Guid id) => Ok(await _teamService.GetByIdAsync(id));

        [HttpPost("create")]
        public async Task<ActionResult<GetTeamDTO>> CreateTeam(CreateTeamDTO teamDto)
        {
            var createdTeam = await _teamService.CreateAsync(teamDto);
            return CreatedAtAction(nameof(GetTeam), new { id = createdTeam.Id }, createdTeam);
        }

        [HttpPost("delete")]
        public async Task<ActionResult> DeleteTeam(Guid id)
        {
            await _teamService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("update")]
        public async Task<ActionResult<GetTeamDTO>> UpdateTeam(Guid id, UpdateTeamDTO teamDto) => Ok(await _teamService.UpdateAsync(id, teamDto));

        [HttpGet("{id:Guid}/matches")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetTeamMatches(Guid id) => Ok(await _teamService.GetMatchesAsync(id));

        [HttpGet("{id:Guid}/players")]
        public async Task<ActionResult<IEnumerable<GetPlayerDTO>>> GetTeamPlayers(Guid id) => Ok(await _teamService.GetPlayersAsync(id));

        [HttpGet("{id:Guid}/fixtures")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetTeamFixtures(Guid id) => Ok(await _teamService.GetFixturesAsync(id));

        [HttpGet("{id:Guid}/results")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetTeamResults(Guid id) => Ok(await _teamService.GetResultsAsync(id));

        [HttpGet("{id:Guid}/last5")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetTeamLast5Matches(Guid id) => Ok(await _teamService.GetLast5MatchesAsync(id));

        [HttpGet("{id:Guid}/form")]
        public async Task<ActionResult<IEnumerable<string>>> GetTeamForm(Guid id) => Ok(await _teamService.GetFormAsync(id));

        [HttpGet("{id:Guid}/statistics")]
        public async Task<ActionResult<GetTeamStatisticsDTO>> GetTeamStatistics(Guid id) => Ok(await _teamService.GetStatisticsAsync(id));

        [HttpGet("{id:Guid}/top-scorers")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetTeamTopScorers(Guid id) => Ok(await _teamService.GetTopScorersAsync(id));

        [HttpGet("{id:Guid}/top-assists")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetTeamTopAssists(Guid id) => Ok(await _teamService.GetTopAssistsAsync(id));

        [HttpGet("{id:Guid}/top-rated")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetTeamTopRated(Guid id) => Ok(await _teamService.GetTopRatedAsync(id));

        [HttpGet("{id:Guid}/most-passes")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetTeamMostPasses(Guid id) => Ok(await _teamService.GetMostPassesAsync(id));

        [HttpGet("{id:Guid}/most-minutes")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetTeamMostMinutes(Guid id) => Ok(await _teamService.GetMostMinutesAsync(id));
    }
}
