using BlazorApp3semesterEksamensProjektMappe.Data;
using BlazorApp3semesterEksamensProjektMappe.Data.Entities;
using BlazorApp3semesterEksamensProjektMappe.shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp3semesterEksamensProjektMappe.Services
{
    public class HutDataService : IHutDataService
    {
        private readonly AppDbContext _db;
        public HutDataService(AppDbContext db) => _db = db;

        public async Task<List<HutDto>> GetAllAsync() =>
            await _db.Huts
                .Include(h => h.Product)
                .Select(h => new HutDto
                {
                    Id = h.Id,
                    MaxCapacity = h.MaxCapacity,
                    ProductId = h.ProductId,
                    Product = new ProductDto
                    {
                        Id = h.Product.Id,
                        ProductType = h.Product.ProductType,
                        SeasonalPrice = h.Product.SeasonalPrice,
                        ServicePrice = h.Product.ServicePrice,
                        NumberOfGuests = h.Product.NumberOfGuests,
                        AdditionalPurchases = h.Product.AdditionalPurchases
                    }
                })
                .ToListAsync();

        public async Task<HutDto?> GetByIdAsync(int id)
        {
            var h = await _db.Huts
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            return h is null ? null : new HutDto
            {
                Id = h.Id,
                MaxCapacity = h.MaxCapacity,
                ProductId = h.ProductId,
                Product = new ProductDto
                {
                    Id = h.Product.Id,
                    ProductType = h.Product.ProductType,
                    SeasonalPrice = h.Product.SeasonalPrice,
                    ServicePrice = h.Product.ServicePrice,
                    NumberOfGuests = h.Product.NumberOfGuests,
                    AdditionalPurchases = h.Product.AdditionalPurchases
                }
            };
        }
    }
}