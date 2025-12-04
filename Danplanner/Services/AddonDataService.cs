using Danplanner.Data;
using Danplanner.Data.Entities;
using Danplanner.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Danplanner.Services
{
    public class AddonDataService(AppDbContext db) : IAddonDataService
    {
        private readonly AppDbContext _db = db;

        public async Task<List<AddonDto>> GetAllAsync()
        {
            return await _db.Addons
                .Select(a => new AddonDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Price = a.Price
                })
                .ToListAsync();
        }

        public async Task<AddonDto?> GetByIdAsync(int id)
        {
            var addon = await _db.Addons.FindAsync(id);
            if (addon == null) return null;

            return new AddonDto
            {
                Id = addon.Id,
                Name = addon.Name,
                Price = addon.Price
            };
        }

        public async Task<AddonDto> CreateAsync(AddonDto dto)
        {
            var addon = new Addons
            {
                Name = dto.Name,
                Price = dto.Price
            };

            _db.Addons.Add(addon);
            await _db.SaveChangesAsync();

            dto.Id = addon.Id;
            return dto;
        }

        public async Task UpdateAsync(int id, AddonDto dto)
        {
            var addon = await _db.Addons.FindAsync(id);
            if (addon == null) return;

            addon.Name = dto.Name;
            addon.Price = dto.Price;

            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var addon = await _db.Addons.FindAsync(id);
            if (addon == null) return;

            _db.Addons.Remove(addon);
            await _db.SaveChangesAsync();
        }
    }
}