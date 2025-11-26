using Danplanner.Data;
using Danplanner.Data.Entities;
using Danplanner.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Danplanner.Services
{
    public class CottageDataService : ICottageDataService
    {
        private readonly AppDbContext _db;
        public CottageDataService(AppDbContext db) => _db = db;

        public async Task<List<CottageDto>> GetAllAsync() =>
            await _db.Cottages
                .Include(h => h.Product)
                .Select(h => new CottageDto
                {
                    Id = h.Id,
                    MaxCapacity = h.MaxCapacity,
                    Number = h.Number,
                    ProductId = h.ProductId,
                    Product = new ProductDto
                    {
                        Id = h.Product.Id,
                        ProductType = h.Product.ProductType,
                        PricePerNight = h.Product.PricePerNight
                        // MaxGuests er fjernet
                    }
                })
                .ToListAsync();

        public async Task<CottageDto?> GetByIdAsync(int id)
        {
            var h = await _db.Cottages
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            return h is null ? null : new CottageDto
            {
                Id = h.Id,
                MaxCapacity = h.MaxCapacity,
                Number = h.Number,
                ProductId = h.ProductId,
                Product = new ProductDto
                {
                    Id = h.Product.Id,
                    ProductType = h.Product.ProductType,
                    PricePerNight = h.Product.PricePerNight
                    // MaxGuests er fjernet
                }
            };
        }
    }
}