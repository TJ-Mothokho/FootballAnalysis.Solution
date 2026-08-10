using FootballAnalysis.Data.Application.DTOs.Common;
using FootballAnalysis.Data.Application.DTOs.Competition;
using FootballAnalysis.Data.Application.DTOs.Match;
using FootballAnalysis.Data.Application.Interfaces;
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
        public async Task<ActionResult<IEnumerable<GetCompetitionDTO>>> GetCompetitions() => Ok(await _competitionService.GetAllAsync());

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<GetCompetitionDTO>> GetCompetition(Guid id) => Ok(await _competitionService.GetByIdAsync(id));

        [HttpPost("create")]
        public async Task<ActionResult<GetCompetitionDTO>> CreateCompetition(CreateCompetitionDTO competitionDto)
        {
            var createdCompetition = await _competitionService.CreateAsync(competitionDto);
            return CreatedAtAction(nameof(GetCompetition), new { id = createdCompetition.Id }, createdCompetition);
        }

        [HttpPut("update")]
        public async Task<ActionResult<GetCompetitionDTO>> UpdateCompetition(Guid id, UpdateCompetitionDTO competitionDto) => Ok(await _competitionService.UpdateAsync(id, competitionDto));

        [HttpDelete("delete/{id:Guid}")]
        public async Task<ActionResult> DeleteCompetition(Guid id)
        {
            await _competitionService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("{id:Guid}/standings")]
        public async Task<ActionResult<IEnumerable<StandingDTO>>> GetStandings(Guid id, [FromQuery] Guid seasonId) => Ok(await _competitionService.GetStandingsAsync(id, seasonId));

        [HttpGet("{id:Guid}/fixtures")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetFixtures(Guid id) => Ok(await _competitionService.GetFixturesAsync(id));

        [HttpGet("{id:Guid}/results")]
        public async Task<ActionResult<IEnumerable<GetMatchDTO>>> GetResults(Guid id) => Ok(await _competitionService.GetResultsAsync(id));

        [HttpGet("{id:Guid}/top-scorers")]
        public async Task<ActionResult<IEnumerable<PlayerLeaderDTO>>> GetTopScorers(Guid id) => Ok(await _competitionService.GetTopScorersAsync(id));

        [HttpGet("{id:Guid}/statistics")]
        public async Task<ActionResult<CompetitionStatisticsDTO>> GetStatistics(Guid id) => Ok(await _competitionService.GetStatisticsAsync(id));
    }
}
