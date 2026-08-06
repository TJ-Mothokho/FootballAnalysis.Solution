using FootballAnalysis.Data.Application.DTOs.Season;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.Interfaces;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<IEnumerable<GetTeamDTO>>> GetTeams()
        {
            try
            {
                var teams = await _teamService.GetAllAsync();
                return Ok(teams);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<GetTeamDTO>> GetTeam(Guid id)
        {
            try
            {
                var team = await _teamService.GetByIdAsync(id);
                if (team == null)
                    return NotFound();
                return Ok(team);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<GetTeamDTO>> CreateTeam(CreateTeamDTO teamDto)
        {
            try
            {
                var createdTeam = await _teamService.CreateAsync(teamDto);
                return CreatedAtAction(nameof(GetTeam), new { id = createdTeam.Id }, createdTeam);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult> DeleteTeam(Guid id)
        {
            try
            {
                await _teamService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<GetTeamDTO>> UpdateTeam(Guid id, UpdateTeamDTO teamDto)
        {
            try
            {
                var updatedTeam = await _teamService.UpdateAsync(id, teamDto);
                return Ok(updatedTeam);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
