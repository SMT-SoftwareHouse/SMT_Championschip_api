using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMTChampionshipAPI.Data;
using SMTChampionshipAPI.DTOS;
using SMTChampionshipAPI.Helpers;
using SMTChampionshipAPI.Models;

namespace SMTChampionshipAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResultsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ResultsController(AppDbContext context)
        {
            _context = context;
        }

        // =========================================
        // GET: api/results
        // Get all results (all teams)
        // =========================================
        [HttpGet]
        public async Task<IActionResult> GetAllResults()
        {
            var teams = await _context.Teams
                .AsNoTracking()
                .ToListAsync();

            if (!teams.Any())
                return Ok(new List<TeamResultDto>());

            var teamIds = teams.Select(t => t.Id).ToList();

            var matches = await _context.Matches
                .Where(m =>
                    m.MatchState == MatchStatus.Done &&
                    m.Team1Id.HasValue &&
                    m.Team2Id.HasValue &&
                    (teamIds.Contains(m.Team1Id.Value) ||
                     teamIds.Contains(m.Team2Id.Value)))
                .AsNoTracking()
                .ToListAsync();

            var results = teams
                .Select(team => ResultHelperCTRL.CalculateTeamResult(team, matches))
                .OrderByDescending(r => r.PointsCount)
                .ThenByDescending(r => r.GoalsCount)
                .ToList();

            return Ok(results);
        }


        // =========================================
        // GET: api/results/by-group/{groupId}
        // =========================================
        [HttpGet("by-group/{groupId}")]
        public async Task<IActionResult> GetByGroup(int groupId)
        {
            var teams = await _context.Teams
                .Where(t => t.GroupId == groupId)
                .AsNoTracking()
                .ToListAsync();

            if (!teams.Any())
                return NotFound("No teams found in this group");

            var teamIds = teams.Select(t => t.Id).ToList();

            var matches = await _context.Matches
                .Where(m =>
                    m.MatchState == MatchStatus.Done &&
                    m.Team1Id.HasValue &&
                    m.Team2Id.HasValue &&
                    (teamIds.Contains(m.Team1Id.Value) ||
                     teamIds.Contains(m.Team2Id.Value)))
                .AsNoTracking()
                .ToListAsync();

            var results = teams
                .Select(team => ResultHelperCTRL.CalculateTeamResult(team, matches))
                .OrderByDescending(r => r.PointsCount)
                .ThenByDescending(r => r.GoalsCount)
                .ToList();

            return Ok(results);
        }

        // =========================================
        // GET: api/results/by-team/{teamId}
        // =========================================
        [HttpGet("by-team/{teamId}")]
        public async Task<IActionResult> GetByTeam(int teamId)
        {
            var team = await _context.Teams
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == teamId);

            if (team == null)
                return NotFound("Team not found");

            var matches = await _context.Matches
                .Where(m =>
                    m.MatchState == MatchStatus.Done &&
                    (m.Team1Id == teamId || m.Team2Id == teamId))
                .AsNoTracking()
                .ToListAsync();

            var result = ResultHelperCTRL.CalculateTeamResult(team, matches);

            return Ok(result);
        }

       
     
    }
}
