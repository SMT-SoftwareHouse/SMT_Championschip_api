using Microsoft.EntityFrameworkCore;
using SMTChampionshipAPI.Models;
using System.Collections.Generic;

namespace SMTChampionshipAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Match> Matches { get; set; }

    }
}
