using FootballAnalysis.Data.Application.DTOs.PlayerMatchStats;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootballAnalysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerMatchStatsController : ControllerBase
    {
        private readonly IPlayerMatchStatsService _playerMatchStatsService;

        public PlayerMatchStatsController(IPlayerMatchStatsService playerMatchStatsService)
        {
            _playerMatchStatsService = playerMatchStatsService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GetPlayerMatchStatsDTO>>> GetPlayerMatchStats()
        {
            try
            {
                var playerMatchStats = await _playerMatchStatsService.GetAllAsync();
                return Ok(playerMatchStats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<GetPlayerMatchStatsDTO>> GetPlayerMatchStats(Guid id)
        {
            try
            {
                var playerMatchStats = await _playerMatchStatsService.GetByIdAsync(id);
                if (playerMatchStats == null)
                    return NotFound();
                return Ok(playerMatchStats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<GetPlayerMatchStatsDTO>> CreatePlayerMatchStats(CreatePlayerMatchStatsDTO playerMatchStatsDto)
        {
            try
            {
                var createdPlayerMatchStats = await _playerMatchStatsService.CreateAsync(playerMatchStatsDto);
                return CreatedAtAction(nameof(GetPlayerMatchStats), new { id = createdPlayerMatchStats.Id }, createdPlayerMatchStats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult> DeletePlayerMatchStats(Guid id)
        {
            try
            {
                await _playerMatchStatsService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<GetPlayerMatchStatsDTO>> UpdatePlayerMatchStats(Guid id, UpdatePlayerMatchStatsDTO playerMatchStatsDto)
        {
            try
            {
                var updatedPlayerMatchStats = await _playerMatchStatsService.UpdateAsync(id, playerMatchStatsDto);
                return Ok(updatedPlayerMatchStats);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
