using BlazorApp3semesterEksamensProjektMappe.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp3semesterEksamensProjektMappe.Data
{
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options) { }

        public DbSet<CampingSite> CampingSites { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Hut> Huts { get; set; }
    }
}