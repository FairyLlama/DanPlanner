using Danplanner.Data;
using Danplanner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Danplanner.Services
{
    public class BookingSeeder
    {
        private readonly AppDbContext _db;
        public BookingSeeder(AppDbContext db) => _db = db;

        public async Task SeedAsync()
        {
            // Seed Resources
            if (!await _db.Resources.AnyAsync())
            {
                _db.Resources.AddRange(
                    new Resource { Name = "Plads 1", Type = "Plads", Location = "A-området" },
                    new Resource { Name = "Plads 2", Type = "Plads", Location = "A-området" },
                    new Resource { Name = "Hytte 3", Type = "Hytte", Location = "B-området" }
                );
            }

            // Seed Products
            if (!await _db.Products.AnyAsync())
            {
                _db.Products.AddRange(
                    new Product { Name = "Plads m/strøm", Description = "Standardplads med el", BasePrice = 150m },
                    new Product { Name = "Hytte standard", Description = "2 pers. hytte", BasePrice = 450m }
                );
            }

            await _db.SaveChangesAsync();
        }
    }

}
