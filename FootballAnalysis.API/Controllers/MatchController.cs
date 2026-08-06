using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.Interfaces;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetMatches()
        {
            try
            {
                var matches = await _matchService.GetAllAsync();
                return Ok(matches);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<GetMatchDTO>> GetMatch(Guid id)
        {
            try
            {
                var match = await _matchService.GetByIdAsync(id);
                if (match == null)
                    return NotFound();
                return Ok(match);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<GetMatchDTO>> CreateMatch(CreateMatchDTO matchDto)
        {
            try
            {
                var createdMatch = await _matchService.CreateAsync(matchDto);
                return CreatedAtAction(nameof(GetMatch), new { id = createdMatch.Id }, createdMatch);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult> DeleteMatch(Guid id)
        {
            try
            {
                await _matchService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<GetMatchDTO>> UpdateMatch(Guid id, UpdateMatchDTO matchDto)
        {
            try
            {
                var updatedMatch = await _matchService.UpdateAsync(id, matchDto);
                return Ok(updatedMatch);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
