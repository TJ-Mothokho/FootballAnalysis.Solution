using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.DTOs.TeamMatchStats;
using FootballAnalysis.Data.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootballAnalysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamMatchStatsController : ControllerBase
    {
        private readonly ITeamMatchStatsService _teamMatchStatsService;

        public TeamMatchStatsController(ITeamMatchStatsService teamMatchStatsService)
        {
            _teamMatchStatsService = teamMatchStatsService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GetTeamMatchStatsDTO>>> GetTeamMatchStats()
        {
            try
            {
                var teamMatchStats = await _teamMatchStatsService.GetAllAsync();
                return Ok(teamMatchStats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<GetTeamMatchStatsDTO>> GetTeamMatchStats(Guid id)
        {
            try
            {
                var teamMatchStats = await _teamMatchStatsService.GetByIdAsync(id);
                if (teamMatchStats == null)
                    return NotFound();
                return Ok(teamMatchStats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<GetTeamMatchStatsDTO>> CreateTeamMatchStats(CreateTeamMatchStatsDTO teamMatchStatsDto)
        {
            try
            {
                var createdTeamMatchStats = await _teamMatchStatsService.CreateAsync(teamMatchStatsDto);
                return CreatedAtAction(nameof(GetTeamMatchStats), new { id = createdTeamMatchStats.Id }, createdTeamMatchStats );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult> DeleteTeamMatchStats(Guid id)
        {
            try
            {
                await _teamMatchStatsService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<GetTeamMatchStatsDTO>> UpdateTeamMatchStats(Guid id, UpdateTeamMatchStatsDTO teamMatchStatsDto)
        {
            try
            {
                var updatedTeamMatchStats = await _teamMatchStatsService.UpdateAsync(id, teamMatchStatsDto);
                return Ok(updatedTeamMatchStats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
