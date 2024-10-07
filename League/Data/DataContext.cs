using League.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace League.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Player> Players { get; set; }

        public DbSet<Club> Clubs { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}
