using BlazorApp3semesterEksamensProjektMappe.Data;
using BlazorApp3semesterEksamensProjektMappe.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp3semesterEksamensProjektMappe.Services
{
    public class BookingSeeder
    {
        private readonly AppDbContext _db;
        public BookingSeeder(AppDbContext db) => _db = db;

        public async Task SeedAsync()
        {
            // Seed Products
            if (!await _db.Products.AnyAsync())
            {
                _db.Products.AddRange(
                    new Product
                    {
                        ProductType = "Plads m/strøm",
                        SeasonalPrice = 150m,
                        ServicePrice = 25m,
                        NumberOfGuests = 4,
                        AdditionalPurchases = "El-tilslutning"
                    },
                    new Product
                    {
                        ProductType = "Hytte standard",
                        SeasonalPrice = 450m,
                        ServicePrice = 50m,
                        NumberOfGuests = 2,
                        AdditionalPurchases = "Slutrengøring"
                    }
                );

                await _db.SaveChangesAsync(); // Vi skal bruge Product.Id til Hut
            }

            // Seed Huts
            if (!await _db.Huts.AnyAsync())
            {
                var hytteProdukt = await _db.Products
                    .FirstOrDefaultAsync(p => p.ProductType.Contains("Hytte"));

                if (hytteProdukt is not null)
                {
                    _db.Huts.AddRange(
                        new Hut { MaxCapacity = 2, ProductId = hytteProdukt.Id },
                        new Hut { MaxCapacity = 4, ProductId = hytteProdukt.Id }
                    );
                }
            }

            await _db.SaveChangesAsync();
        }
    }
}