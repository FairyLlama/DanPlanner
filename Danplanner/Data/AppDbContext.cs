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

        public DbSet<Cottage> Cottages { get; set; }

        public DbSet<GrassField> GrassFields { get; set; }

        public DbSet<Addons> Addons { get; set; }

        // Manglende tabeller:
        public DbSet<BookingAddon> BookingAddons { get; set; }
        public DbSet<Receipt> Receipts { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>()
                .Property(p => p.ProductType)
                .HasConversion<string>(); // gemmer enum-værdier som tekst i DB
        }
    }



}