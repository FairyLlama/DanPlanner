using Danplanner.Data;
using Danplanner.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Danplanner.Services
{
    public class BookingSeeder
    {
        private readonly AppDbContext _db;

        public BookingSeeder(AppDbContext db)
        {
            _db = db;
        }

        public async Task SeedAsync()
        {
            // --- Products ---
            await EnsureProduct("Græsplads m/strøm", 150m);
            await EnsureProduct("Hytte standard", 450m);
            await EnsureProduct("Hytte luksus", 950m);

            await _db.SaveChangesAsync();

            // --- Cottages ---
            var cottageProduct = await _db.Products.FirstOrDefaultAsync(p => p.ProductType.Contains("Hytte"));
            if (cottageProduct is not null)
            {
                await EnsureCottage(cottageProduct, 2, 3);
                await EnsureCottage(cottageProduct, 4, 6);
            }

            // --- GrassFields ---
            var grassProduct = await _db.Products.FirstOrDefaultAsync(p => p.ProductType.Contains("Græsplads"));
            if (grassProduct is not null)
            {
                await EnsureGrassField(grassProduct, "50m2", 5);
                await EnsureGrassField(grassProduct, "70m2", 8);
            }

            // --- Addons ---
            await EnsureAddon("Slutrengøring", 300m);
            await EnsureAddon("Sengetøj", 75m);
            await EnsureAddon("Cykelleje", 150m);

            await _db.SaveChangesAsync();

            // --- Bookings (eksempeldata) ---
            if (!await _db.Bookings.AnyAsync())
            {
                var productHytte = await _db.Products.FirstOrDefaultAsync(p => p.ProductType.Contains("Hytte"));
                var productGræs = await _db.Products.FirstOrDefaultAsync(p => p.ProductType.Contains("Græsplads"));
                var cottage = await _db.Cottages.FirstOrDefaultAsync();
                var grassField = await _db.GrassFields.FirstOrDefaultAsync();
                var addon = await _db.Addons.FirstOrDefaultAsync();

                if (productHytte is not null && cottage is not null)
                {
                    var bookingHytte = new Booking
                    {
                        ProductId = productHytte.Id,
                        Product = productHytte,
                        CottageId = cottage.Id,
                        Cottage = cottage,
                        StartDate = DateTime.Today.AddDays(2),
                        EndDate = DateTime.Today.AddDays(5),
                        Status = "Aktiv",
                        NumberOfPeople = 2 // eksempel antal personer
                    };

                    bookingHytte.BookingAddons = new List<BookingAddon>
                    {
                        new BookingAddon
                        {
                            AddonId = addon!.Id,
                            Addons = addon,
                            Booking = bookingHytte,
                            Quantity = 2
                        }
                    };

                    _db.Bookings.Add(bookingHytte);
                }

                if (productGræs is not null && grassField is not null)
                {
                    var bookingGræs = new Booking
                    {
                        ProductId = productGræs.Id,
                        Product = productGræs,
                        GrassFieldId = grassField.Id,
                        GrassField = grassField,
                        StartDate = DateTime.Today.AddDays(7),
                        EndDate = DateTime.Today.AddDays(10),
                        Status = "Aktiv",
                        NumberOfPeople = 3 // eksempel antal personer
                    };

                    bookingGræs.BookingAddons = new List<BookingAddon>
                    {
                        new BookingAddon
                        {
                            AddonId = addon!.Id,
                            Addons = addon,
                            Booking = bookingGræs,
                            Quantity = 1
                        }
                    };

                    _db.Bookings.Add(bookingGræs);
                }
            }

            await _db.SaveChangesAsync();
        }

        // --- Helper methods ---
        private async Task EnsureProduct(string type, decimal price)
        {
            var existing = await _db.Products.FirstOrDefaultAsync(p => p.ProductType == type);
            if (existing is null)
            {
                _db.Products.Add(new Product
                {
                    ProductType = type,
                    PricePerNight = price
                });
            }
            else
            {
                existing.PricePerNight = price;
            }
        }

        private async Task EnsureCottage(Product product, int capacity, int number)
        {
            var existing = await _db.Cottages.FirstOrDefaultAsync(c => c.Number == number);
            if (existing is null)
            {
                _db.Cottages.Add(new Cottage
                {
                    ProductId = product.Id,
                    Product = product,
                    MaxCapacity = capacity,
                    Number = number
                });
            }
            else
            {
                existing.MaxCapacity = capacity;
                existing.ProductId = product.Id;
                existing.Product = product;
            }
        }

        private async Task EnsureGrassField(Product product, string size, int number)
        {
            var existing = await _db.GrassFields.FirstOrDefaultAsync(g => g.Number == number);
            if (existing is null)
            {
                _db.GrassFields.Add(new GrassField
                {
                    ProductId = product.Id,
                    Product = product,
                    Size = size,
                    Number = number
                });
            }
            else
            {
                existing.Size = size;
                existing.ProductId = product.Id;
                existing.Product = product;
            }
        }

        private async Task EnsureAddon(string name, decimal price)
        {
            var existing = await _db.Addons.FirstOrDefaultAsync(a => a.Name == name);
            if (existing is null)
            {
                _db.Addons.Add(new Addons
                {
                    Name = name,
                    Price = price
                });
            }
            else
            {
                existing.Price = price;
            }
        }
    }
}