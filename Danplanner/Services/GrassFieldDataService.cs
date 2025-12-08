using Danplanner.Data;
using Danplanner.Data.Entities;
using Danplanner.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Danplanner.Services
{
    public class GrassFieldDataService(AppDbContext db) : IGrassFieldDataService
    {
        private readonly AppDbContext _db = db;

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
                    MaxCapacity = g.MaxCapacity,        // 🔥 TILFØJET
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
                MaxCapacity = g.MaxCapacity,        // 🔥 TILFØJET
                Product = new ProductDto
                {
                    Id = g.Product.Id,
                    ProductType = (ProductType)g.Product.ProductType
                }
            };
        }

        // CREATE
        public async Task<GrassFieldDto> CreateAsync(GrassFieldDto dto)
        {
            var entity = new GrassField
            {
                ProductId = dto.ProductId,
                Size = dto.Size,
                Number = dto.Number,
                PricePerNight = dto.PricePerNight,
                MaxCapacity = dto.MaxCapacity,      // 🔥 TILFØJET
                Product = await _db.Products.FindAsync(dto.ProductId)
            };

            _db.GrassFields.Add(entity);
            await _db.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        // UPDATE
        public async Task<GrassFieldDto> UpdateAsync(int id, GrassFieldDto dto)
        {
            var entity = await _db.GrassFields.FindAsync(id);
            if (entity is null) throw new KeyNotFoundException($"GrassField {id} not found");

            entity.ProductId = dto.ProductId;
            entity.Size = dto.Size;
            entity.Number = dto.Number;
            entity.PricePerNight = dto.PricePerNight;
            entity.MaxCapacity = dto.MaxCapacity;  // 🔥 TILFØJET

            await _db.SaveChangesAsync();
            return dto;
        }

        // DELETE
        public async Task DeleteAsync(int id)
        {
            var entity = await _db.GrassFields.FindAsync(id);
            if (entity is null) throw new KeyNotFoundException($"GrassField {id} not found");

            _db.GrassFields.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}
