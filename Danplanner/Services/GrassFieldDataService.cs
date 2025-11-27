using Danplanner.Data;
using Danplanner.Data.Entities;
using Danplanner.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Danplanner.Services
{
    public class GrassFieldDataService : IGrassFieldDataService
    {
        private readonly AppDbContext _db;
        public GrassFieldDataService(AppDbContext db) => _db = db;

        public async Task<List<GrassFieldDto>> GetAllAsync() =>
            await _db.GrassFields
                .Include(g => g.Product)
                .Select(g => new GrassFieldDto
                {
                    Id = g.Id,
                    ProductId = g.ProductId,
                    Size = g.Size,
                    Number = g.Number,
                    PricePerNight = g.PricePerNight,
                    Product = new ProductDto
                    {
                        Id = g.Product.Id,
                        ProductType = (ProductType)g.Product.ProductType
                    }
                })
                .ToListAsync();

        public async Task<GrassFieldDto?> GetByIdAsync(int id)
        {
            var g = await _db.GrassFields
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            return g is null ? null : new GrassFieldDto
            {
                Id = g.Id,
                ProductId = g.ProductId,
                Size = g.Size,
                Number = g.Number,
                PricePerNight = g.PricePerNight,
                Product = new ProductDto
                {
                    Id = g.Product.Id,
                    ProductType = (ProductType)g.Product.ProductType
                }
            };
        }
    }
}