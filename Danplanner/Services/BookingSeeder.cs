using Danplanner.Data;
using Danplanner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Danplanner.Services
{
    public class BookingSeeder(AppDbContext db)
    {
        private readonly AppDbContext _db = db;

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

                await _db.SaveChangesAsync(); // Vi skal bruge Product.Id til Cottage
            }

            // Seed Cottages
            if (!await _db.Cottages.AnyAsync())
            {
                var cottageProduct = await _db.Products
                    .FirstOrDefaultAsync(p => p.ProductType.Contains("Hytte"));

                if (cottageProduct is not null)
                {
                    _db.Cottages.AddRange(
                        new Cottage { MaxCapacity = 2, ProductId = cottageProduct.Id, Product = cottageProduct },
                        new Cottage { MaxCapacity = 4, ProductId = cottageProduct.Id, Product = cottageProduct }
                    );
                }
            }

            await _db.SaveChangesAsync();
        }
    }
}