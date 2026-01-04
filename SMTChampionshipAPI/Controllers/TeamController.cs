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
    public class TeamController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        IConfiguration configuration;
        public TeamController(IConfiguration configuration, AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
            this.configuration = configuration;
        }

        // =========================
        // GET: api/team
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(_context.Teams.ToList());
        }

        // =========================
        // GET: api/team/{id}
        // =========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound();

            return Ok(team);
        }

        // =========================
        // POST: api/team (Admin)
        // =========================
        [HttpPost]

        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(
            [FromForm] TeamInputDto teamInput)
        {
            Team team = new Team()
            {
                ArDescription = teamInput.ArDescription,
                ArName = teamInput.ArName,
                EnDescription = teamInput.EnDescription,
                EnName = teamInput.EnName
            };
            team.LogoPath = await LogoHelpersCTRL.SaveLogoAsync(configuration, teamInput.Logo);


            _context.Teams.Add(team);
            await _context.SaveChangesAsync();

            return Ok(team);
        }

        // =========================
        // PUT: api/team/{id} (Admin)
        // =========================
        [HttpPut("{id}")]

        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(
            int id,
            [FromForm] TeamInputDto teamInput)
        {


            var existing = await _context.Teams.FindAsync(id);
            if (existing == null)
                return NotFound();

            // Update fields
            existing.EnName = teamInput.EnName;
            existing.ArName = teamInput.ArName;
            existing.EnDescription = teamInput.EnDescription;
            existing.ArDescription = teamInput.ArDescription;


            existing.LogoPath = await LogoHelpersCTRL.SaveLogoAsync(configuration, teamInput.Logo);


            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // =========================
        // DELETE: api/team/{id} (Admin)
        // =========================
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
                return NotFound();


            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return NoContent();
        }




    }
}
