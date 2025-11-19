using BlazorApp3semesterEksamensProjektMappe.Data;
using BlazorApp3semesterEksamensProjektMappe.shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp3semesterEksamensProjektMappe.Services
{
    public class ProductDataService : IProductDataService
    {
        private readonly AppDbContext _db;
        public ProductDataService(AppDbContext db) => _db = db;

        public async Task<List<ProductDto>> GetAllAsync() =>
            await _db.Products.Select(p => new ProductDto
            {
                Id = p.Id,
                ProductType = p.ProductType,
                SeasonalPrice = p.SeasonalPrice,
                ServicePrice = p.ServicePrice,
                NumberOfGuests = p.NumberOfGuests,
                AdditionalPurchases = p.AdditionalPurchases
            }).ToListAsync();

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var p = await _db.Products.FindAsync(id);
            return p is null ? null : new ProductDto
            {
                Id = p.Id,
                ProductType = p.ProductType,
                SeasonalPrice = p.SeasonalPrice,
                ServicePrice = p.ServicePrice,
                NumberOfGuests = p.NumberOfGuests,
                AdditionalPurchases = p.AdditionalPurchases
            };
        }
    }
}