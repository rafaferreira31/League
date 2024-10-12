using League.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace League.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Player> Players { get; set; }

        public DbSet<Staff> Staffs { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<Game> Games { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
