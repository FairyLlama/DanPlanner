using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Danplanner.Data.Entities;

namespace Danplanner.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CampingSite> CampingSites { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Resource> Resources { get; set; }
    }
}
