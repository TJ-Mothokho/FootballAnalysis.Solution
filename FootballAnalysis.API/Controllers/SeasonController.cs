using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.DTOs.Player;
using FootballAnalysis.Data.Application.DTOs.Season;
using FootballAnalysis.Data.Application.DTOs.Team;
using FootballAnalysis.Data.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FootballAnalysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeasonController : ControllerBase
    {
        private readonly ISeasonService _seasonService;

        public SeasonController(ISeasonService seasonService)
        {
            _seasonService = seasonService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GetSeasonDTO>>> GetSeasons() => Ok(await _seasonService.GetAllAsync());

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<GetSeasonDTO>> GetSeason(Guid id) => Ok(await _seasonService.GetByIdAsync(id));

        [HttpPost("create")]
        public async Task<ActionResult<GetSeasonDTO>> CreateSeason(CreateSeasonDTO seasonDto)
        {
            var createdSeason = await _seasonService.CreateAsync(seasonDto);
            return CreatedAtAction(nameof(GetSeason), new { id = createdSeason.Id }, createdSeason);
        }

        [HttpPost("delete")]
        public async Task<ActionResult> DeleteSeason(Guid id)
        {
            await _seasonService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("update")]
        public async Task<ActionResult<GetSeasonDTO>> UpdateSeason(Guid id, UpdateSeasonDTO seasonDto) => Ok(await _seasonService.UpdateAsync(id, seasonDto));

        [HttpGet("{id:Guid}/statistics")]
        public async Task<ActionResult<GetSeasonStatisticsDTO>> GetSeasonStatistics(Guid id) => Ok(await _seasonService.GetSeasonStatisticsAsync(id));

        [HttpGet("{id:Guid}/matches")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetSeasonMatches(Guid id) => Ok(await _seasonService.GetSeasonMatchesAsync(id));

        [HttpGet("{id:Guid}/players")]
        public async Task<ActionResult<IEnumerable<GetPlayerDTO>>> GetSeasonPlayers(Guid id) => Ok(await _seasonService.GetSeasonPlayersAsync(id));

        [HttpGet("{id:Guid}/teams")]
        public async Task<ActionResult<IEnumerable<GetTeamDTO>>> GetSeasonTeams(Guid id) => Ok(await _seasonService.GetSeasonTeamsAsync(id));

        [HttpGet("{id:Guid}/leaders")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetSeasonLeaders(Guid id) => Ok(await _seasonService.GetSeasonLeadersAsync(id));
    }
}
