using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SMTChampionshipAPI.Data;
using SMTChampionshipAPI.Models;

namespace SMTChampionshipAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GroupsController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET: api/groups
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var groups = await _context.Groups
                .AsNoTracking()
                .ToListAsync();


            return Ok(new { groups = groups.Select(s => new { group = s, teams = _context.Teams.Where(w => w.GroupId == s.Id).ToList() }) });
        }

        // =========================
        // GET: api/groups/{id}
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var _group = await _context.Groups.FindAsync(id);
            if (_group == null)
                return NotFound();

            return Ok(new { group = _group, teams = _context.Teams.Where(w => w.GroupId == _group.Id).ToList() });
        }

        // =========================
        // POST: api/groups (Admin)
        // =========================
        [HttpPost]

        public async Task<IActionResult> Create(string groupName)
        {
            Group group = new Models.Group() { Name = groupName };
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return Ok(group);
        }

        // =========================
        // PUT: api/groups/{id} (Admin)
        // =========================
        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, string name)
        {
            

            var existing = await _context.Groups.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.Name = name;
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        // =========================
        // DELETE: api/groups/{id} (Admin)
        // =========================
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var group = await _context.Groups.FindAsync(id);
            if (group == null)
                return NotFound();

            _context.Groups.Remove(group);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // =====================================================
        // POST: api/groups/{groupId}/teams/{teamId}
        // Add Team to Group (Admin)
        // =====================================================
        [HttpPost("{groupId}/teams/{teamId}")]

        public async Task<IActionResult> AddTeamToGroup(int groupId, int teamId)
        {
            var group = await _context.Groups.FindAsync(groupId);
            if (group == null)
                return NotFound("Group not found");

            var team = await _context.Teams.FindAsync(teamId);
            if (team == null)
                return NotFound("Team not found");

            team.GroupId = groupId;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Team added to group successfully",
                GroupId = groupId,
                TeamId = teamId
            });
        }

        // =====================================================
        // DELETE: api/groups/{groupId}/teams/{teamId}
        // Remove Team from Group (Admin)
        // =====================================================
        [HttpDelete("{groupId}/teams/{teamId}")]

        public async Task<IActionResult> RemoveTeamFromGroup(int groupId, int teamId)
        {
            var team = await _context.Teams
                .FirstOrDefaultAsync(t => t.Id == teamId && t.GroupId == groupId);

            if (team == null)
                return NotFound("Team not assigned to this group");

            team.GroupId = null;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Team removed from group successfully"
            });
        }



    }
}
