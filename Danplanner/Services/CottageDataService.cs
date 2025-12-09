using Danplanner.Data;
using Danplanner.Data.Entities;
using Danplanner.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Danplanner.Services
{
    public class CottageDataService(AppDbContext db) : ICottageDataService
    {
        private readonly AppDbContext _db = db;

        // Hent alle hytter
        public async Task<List<CottageDto>> GetAllAsync() =>
            await _db.Cottages
                .Include(h => h.Product)
                .Select(h => new CottageDto
                {
                    Id = h.Id,
                    MaxCapacity = h.MaxCapacity,
                    Number = h.Number,
                    ProductId = h.ProductId,
                    PricePerNight = h.PricePerNight,
                    Product = new ProductDto
                    {
                        Id = h.Product.Id,
                        ProductType = (ProductType)h.Product.ProductType
                    }
                })
                .ToListAsync();


        // Hent hytte efter ID
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
                PricePerNight = h.PricePerNight,
                Product = new ProductDto
                {
                    Id = h.Product.Id,
                    ProductType = (ProductType)h.Product.ProductType
                }
            };
        }

        // opret hytte
        public async Task<CottageDto> CreateAsync(CottageDto dto)
        {
            var entity = new Cottage
            {
                ProductId = dto.ProductId,
                MaxCapacity = dto.MaxCapacity,
                Number = dto.Number,
                PricePerNight = dto.PricePerNight,
                Product = await _db.Products.FindAsync(dto.ProductId)
            };
            _db.Cottages.Add(entity);
            await _db.SaveChangesAsync();
            dto.Id = entity.Id;
            return dto;
        }

        // opdater hytte
        public async Task<CottageDto> UpdateAsync(int id, CottageDto dto)
        {
            var entity = await _db.Cottages.FindAsync(id);
            if (entity is null)
                throw new KeyNotFoundException($"Cottage with ID {id} not found.");

            entity.ProductId = dto.ProductId;
            entity.MaxCapacity = dto.MaxCapacity;
            entity.Number = dto.Number;
            entity.PricePerNight = dto.PricePerNight;

            _db.Cottages.Update(entity);
            await _db.SaveChangesAsync();

            dto.Id = entity.Id;
            return dto;
        }

        // slet hytte
        public async Task DeleteAsync(int id)
        {
            var entity = await _db.Cottages.FindAsync(id);
            if (entity is null)
                throw new KeyNotFoundException($"Cottage with ID {id} not found.");

            _db.Cottages.Remove(entity);
            await _db.SaveChangesAsync();
        }
    }
}