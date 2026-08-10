using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Player;
using FootballAnalysis.Data.Application.Interfaces;
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
        public async Task<ActionResult<IEnumerable<GetPlayerDTO>>> GetPlayers() => Ok(await _playerService.GetAllAsync());

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<GetPlayerDTO>> GetPlayer(Guid id) => Ok(await _playerService.GetByIdAsync(id));

        [HttpPost("create")]
        public async Task<ActionResult<GetPlayerDTO>> CreatePlayer(CreatePlayerDTO playerDto)
        {
            var createdPlayer = await _playerService.CreateAsync(playerDto);
            return CreatedAtAction(nameof(GetPlayer), new { id = createdPlayer.Id }, createdPlayer);
        }

        [HttpPost("delete")]
        public async Task<ActionResult> DeletePlayer(Guid id)
        {
            await _playerService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("update")]
        public async Task<ActionResult<GetPlayerDTO>> UpdatePlayer(Guid id, UpdatePlayerDTO playerDto) => Ok(await _playerService.UpdateAsync(id, playerDto));

        [HttpGet("{id:Guid}/matches")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetPlayerMatches(Guid id) => Ok(await _playerService.GetMatchesAsync(id));

        [HttpGet("{id:Guid}/statistics")]
        public async Task<ActionResult<PlayerStatisticsDTO>> GetPlayerStatistics(Guid id, [FromQuery] Guid seasonId) => Ok(await _playerService.GetStatisticsAsync(id, seasonId));

        [HttpGet("{id:Guid}/season-stats")]
        public async Task<ActionResult<PlayerStatisticsDTO>> GetPlayerSeasonStats(Guid id, [FromQuery] Guid seasonId) => Ok(await _playerService.GetSeasonStatsAsync(id, seasonId));

        [HttpGet("{id:Guid}/last5")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetPlayerLast5(Guid id) => Ok(await _playerService.GetLast5MatchesAsync(id));

        [HttpGet("{id:Guid}/ratings")]
        public async Task<ActionResult<IEnumerable<double>>> GetPlayerRatings(Guid id) => Ok(await _playerService.GetRatingsAsync(id));

        [HttpGet("{id:Guid}/goals")]
        public async Task<ActionResult<int>> GetPlayerGoals(Guid id) => Ok(await _playerService.GetGoalsAsync(id));

        [HttpGet("{id:Guid}/assists")]
        public async Task<ActionResult<int>> GetPlayerAssists(Guid id) => Ok(await _playerService.GetAssistsAsync(id));

        [HttpGet("{id:Guid}/comparison/{otherPlayerId:Guid}")]
        public async Task<ActionResult<PlayerComparisonDTO>> ComparePlayers(Guid id, Guid otherPlayerId) => Ok(await _playerService.CompareAsync(id, otherPlayerId));
    }
}
