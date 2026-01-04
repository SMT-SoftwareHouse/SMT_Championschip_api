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
    public class MatchesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MatchesController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET: api/matches
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = _context.Matches
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .AsNoTracking()
                .AsQueryable();


            var matches = await query
                .OrderBy(m => m.MatchDateTime)
                .ToListAsync();

            return Ok(matches.Select(MatchHelperCTRL.MapMatch));
        }

        // =========================
        // GET: api/matches/by-group/{groupId}
        // =========================
        [HttpGet("by-group/{groupId}")]
        public async Task<IActionResult> GetByGroupId(int groupId)
        {
            // تأكد أن المجموعة موجودة
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == groupId);
            if (!groupExists)
                return NotFound("Group not found");

            var matches = await _context.Matches
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .Where(m =>
                    m.Team1Id.HasValue &&
                    m.Team2Id.HasValue &&
                    m.Team1 != null &&
                    m.Team2 != null &&
                    m.Team1.GroupId == groupId &&
                    m.Team2.GroupId == groupId)
                .OrderBy(m => m.MatchDateTime)
                .AsNoTracking()
                .ToListAsync();

            return Ok(matches.Select(MatchHelperCTRL.MapMatch));
        }

        // =========================
        // GET: api/matches/{id}
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var match = await _context.Matches
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (match == null)
                return NotFound();

            return Ok(MatchHelperCTRL.MapMatch(match));
        }

        // =========================
        // GET: api/matches/by-team/{teamId}
        // =========================
        [HttpGet("by-team/{teamId}")]
        public async Task<IActionResult> GetByTeamId(int teamId)
        {
            var exists = await _context.Teams.AnyAsync(t => t.Id == teamId);
            if (!exists)
                return NotFound("Team not found");

            var matches = await _context.Matches
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .Where(m => m.Team1Id == teamId || m.Team2Id == teamId)
                .OrderBy(m => m.MatchDateTime)
                .AsNoTracking()
                .ToListAsync();

            return Ok(matches.Select(MatchHelperCTRL.MapMatch));
        }

        // =========================
        // POST: api/matches (Admin)
        // =========================
        [HttpPost]

        public async Task<IActionResult> Create([FromBody] MatchCreateDTO newMatch)
        {
            

         

            Match match = new Match();
            match.Team1Id = newMatch.Team1Id;
            match.Team2Id = newMatch.Team2Id;
            match.Team1 = _context.Teams.Where(w => w.Id == newMatch.Team1Id).FirstOrDefault();
            match.Team2 = _context.Teams.Where(w => w.Id == newMatch.Team2Id).FirstOrDefault();
            match.Name = newMatch.MatchName;
            match.MatchDateTime = newMatch.MatchDateTime;

            _context.Matches.Add(match);
            await _context.SaveChangesAsync();

            return Ok(match);
        }

        // =========================
        // PUT: api/matches/{id} (Admin)
        // =========================
        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, [FromBody] Match match)
        {
            if (id != match.Id)
                return BadRequest("Id mismatch");

            if (match.Team1Id == match.Team2Id)
                return BadRequest("Team1 and Team2 cannot be the same");

            var existing = await _context.Matches.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Team1Id = match.Team1Id;
            existing.Team2Id = match.Team2Id;
            existing.Team1Goals = match.Team1Goals;
            existing.Team2Goals = match.Team2Goals;
            existing.MatchDateTime = match.MatchDateTime;
            existing.Active = match.Active;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // =========================
        // DELETE: api/matches/{id} (Admin)
        // =========================
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
                return NotFound();

            _context.Matches.Remove(match);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // =========================
        // PATCH: api/matches/{id}/time (Admin)
        // Change match time only
        // =========================
        [HttpPatch("{id}/time")]

        public async Task<IActionResult> UpdateMatchTime(
            int id,
            [FromBody] UpdateMatchTimeDto dto)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
                return NotFound("Match not found");

            match.MatchDateTime = dto.MatchDateTime;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Match time updated successfully",
                MatchId = match.Id,
                NewTime = match.MatchDateTime
            });
        }

        // =========================
        // PATCH: api/matches/{id}/assign-team (Admin)
        // Assign team to match slot (Team1 / Team2)
        // =========================
        [HttpPatch("{id}/assign-team")]

        public async Task<IActionResult> AssignTeamToMatch(
            int id,
            [FromBody] AssignTeamToMatchDto dto)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
                return NotFound("Match not found");

            var teamExists = await _context.Teams.AnyAsync(t => t.Id == dto.TeamId);
            if (!teamExists)
                return NotFound("Team not found");

            if (string.IsNullOrEmpty(dto.Slot))
                return BadRequest("Slot must be 'Team1' or 'Team2'");

            string slot = dto.Slot.ToLower();

            if (slot != "team1" && slot != "team2")
                return BadRequest("Slot must be 'Team1' or 'Team2'");



            if (slot == "team1")
            {
                if (match.Team2Id == dto.TeamId)
                    return BadRequest("Same team cannot be assigned twice");

                match.Team1Id = dto.TeamId;
            }
            else
            {
                if (match.Team1Id == dto.TeamId)
                    return BadRequest("Same team cannot be assigned twice");

                match.Team2Id = dto.TeamId;
            }


            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Team assigned to match successfully",
                MatchId = match.Id,
                AssignedTeamId = dto.TeamId,
                Slot = dto.Slot
            });
        }

        // =========================
        // PATCH: api/matches/{id}/set-goals
        // =========================
        [HttpPatch("{id}/set-goals")]

        public async Task<IActionResult> SetMatchGoals(
            int id,
            [FromBody] SetMatchGoalsDto dto)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
                return NotFound("Match not found");

            if (match.MatchState == MatchStatus.Done)
                return BadRequest("Cannot set goals for a finished match");

            match.Team1Goals = dto.Team1Goals;
            match.Team2Goals = dto.Team2Goals;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Match goals updated successfully",
                MatchId = match.Id,
                match.Team1Goals,
                match.Team2Goals
            });
        }
        // =========================
        // PATCH: api/matches/{id}/set-status
        // =========================
        [HttpPatch("{id}/set-status")]

        public async Task<IActionResult> SetMatchStatus(
            int id,
            [FromBody] SetMatchStatusDto dto)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match == null)
                return NotFound("Match not found");

            if (match.MatchState == MatchStatus.Done)
                return BadRequest("Match is already finished");

            // Validation of transitions
            if (dto.Status == MatchStatus.Done &&
                match.MatchState != MatchStatus.InProgress)
            {
                return BadRequest("Match must be InProgress before setting it to Done");
            }

            match.MatchState = dto.Status;

            // Optional: auto-activate/deactivate
            if (dto.Status == MatchStatus.InProgress)
                match.Active = true;

            if (dto.Status == MatchStatus.Done)
                match.Active = false;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Match status updated successfully",
                MatchId = match.Id,
                NewStatus = match.MatchState.ToString()
            });
        }



    }
}
