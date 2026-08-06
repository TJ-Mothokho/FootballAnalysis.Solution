using FootballAnalysis.Data.Application.DTOs.Season;
using FootballAnalysis.Data.Application.Interfaces;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<IEnumerable<GetSeasonDTO>>> GetSeasons()
        {
            try
            {
                var seasons = await _seasonService.GetAllAsync();
                return Ok(seasons);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<GetSeasonDTO>> GetSeason(Guid id)
        {
            try
            {
                var season = await _seasonService.GetByIdAsync(id);
                if (season == null)
                    return NotFound();
                return Ok(season);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<GetSeasonDTO>> CreateSeason(CreateSeasonDTO seasonDto)
        {
            try
            {
                var createdSeason = await _seasonService.CreateAsync(seasonDto);
                return CreatedAtAction(nameof(GetSeason), new { id = createdSeason.Id }, createdSeason);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete")]
        public async Task<ActionResult> DeleteSeason(Guid id)
        {
            try
            {
                await _seasonService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<GetSeasonDTO>> UpdateSeason(Guid id, UpdateSeasonDTO seasonDto)
        {
            try
            {
                var updatedSeason = await _seasonService.UpdateAsync(id, seasonDto);
                return Ok(updatedSeason);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
