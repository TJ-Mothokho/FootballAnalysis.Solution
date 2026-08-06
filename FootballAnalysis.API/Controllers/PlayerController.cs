using FootballAnalysis.Data.Application.DTOs.Player;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootballAnalysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly IPlayerService _playerService;

        public PlayerController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GetPlayerDTO>>> GetPlayers()
        {
            try
            {
                var players = await _playerService.GetAllAsync();
                return Ok(players);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<GetPlayerDTO>> GetPlayer(Guid id)
        {
            try
            {
                var player = await _playerService.GetByIdAsync(id);
                if (player == null)
                    return NotFound();
                return Ok(player);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<GetPlayerDTO>> CreatePlayer(CreatePlayerDTO playerDto)
        {
            try
            {
                var createdPlayer = await _playerService.CreateAsync(playerDto);
                return CreatedAtAction(nameof(GetPlayer), new { id = createdPlayer.Id }, createdPlayer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult> DeletePlayer(Guid id)
        {
            try
            {
                await _playerService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<GetPlayerDTO>> UpdatePlayer(Guid id, UpdatePlayerDTO playerDto)
        {
            try
            {
                var updatedPlayer = await _playerService.UpdateAsync(id, playerDto);
                return Ok(updatedPlayer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
