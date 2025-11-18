using Danplanner.Data;
using Danplanner.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Danplanner.Services
{
    public class ProductDataService : IProductDataService
    {
        private readonly AppDbContext _db;
        public ProductDataService(AppDbContext db) => _db = db;

        public async Task<List<ProductDto>> GetAllAsync() =>
            await _db.Products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                BasePrice = p.BasePrice
            }).ToListAsync();

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var p = await _db.Products.FindAsync(id);
            return p is null ? null : new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                BasePrice = p.BasePrice
            };
        }
    }

}
