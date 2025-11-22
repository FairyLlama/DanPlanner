using Danplanner.Data;
using Danplanner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Danplanner.Services
{
    public class CampingSiteSeeder
    {
        private readonly AppDbContext _db;
        public CampingSiteSeeder(AppDbContext db) => _db = db;

        public async Task SeedAsync()
        {
            // Hvis der allerede findes data, så gør ingenting
            if (await _db.CampingSites.AnyAsync()) return;

            var rnd = new Random(42);
            var regions = new[] { "Nordjylland", "Midtjylland", "Syddanmark", "Sjælland", "Bornholm" };

            var sites = Enumerable.Range(1, 50).Select(i => new CampingSite
            {
                Name = $"Campingplads {i}",
                Region = regions[rnd.Next(regions.Length)],
                Latitude = 54.56 + rnd.NextDouble() * 4.0,   // ca. DK breddegrader
                Longitude = 8.07 + rnd.NextDouble() * 6.0,   // ca. DK længdegrader
                HasWifi = rnd.Next(2) == 0,
                HasPlayground = rnd.Next(2) == 0,
                HasBeach = rnd.Next(2) == 0
            }).ToList();

            _db.CampingSites.AddRange(sites);
            await _db.SaveChangesAsync();
        }
    }
}
