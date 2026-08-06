using FootballAnalysis.Data.Application.DTOs.Competition;
using FootballAnalysis.Data.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FootballAnalysis.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitionController : ControllerBase
    {
        private readonly ICompetitionService _competitionService;

        public CompetitionController(ICompetitionService competitionService)
        {
            _competitionService = competitionService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<GetCompetitionDTO>>> GetCompetitions()
        {
            try
            {
                var competitions = await _competitionService.GetAllAsync();
                return Ok(competitions);
            }
            catch
            {
                return BadRequest("Failed to retrieve competitions.");
            }
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<GetCompetitionDTO>> GetCompetition(Guid id)
        {
            try
            {
                var competition = await _competitionService.GetByIdAsync(id);
                if (competition == null)
                    return NotFound();

                return Ok(competition);
            }
            catch
            {
                return BadRequest("Failed to retrieve the competition.");
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<GetCompetitionDTO>> CreateCompetition(CreateCompetitionDTO competitionDto)
        {
            try
            {
                var createdCompetition = await _competitionService.CreateAsync(competitionDto);
                return CreatedAtAction(nameof(GetCompetition), new { id = createdCompetition.Id }, createdCompetition);
            }
            catch
            {
                return BadRequest("Failed to create the competition.");
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<GetCompetitionDTO>> UpdateCompetition(Guid id, UpdateCompetitionDTO competitionDto)
        {
            try
            {
                var updatedCompetition = await _competitionService.UpdateAsync(id, competitionDto);
                if (updatedCompetition == null)
                    return NotFound();

                return Ok(updatedCompetition);
            }
            catch
            {
                return BadRequest("Failed to update the competition.");
            }
        }

        [HttpDelete("delete/{id:Guid}")]
        public async Task<ActionResult> DeleteCompetition(Guid id)
        {
            try
            {
                var result = await _competitionService.DeleteAsync(id);
                if (!result)
                    return NotFound();

                return NoContent();
            }
            catch
            {
                return BadRequest("Failed to delete the competition.");
            }
        }
    }
}
