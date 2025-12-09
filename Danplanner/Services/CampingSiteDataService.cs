using Danplanner.Data;
using Danplanner.Data.Entities;
using Danplanner.Shared.Models;
using Microsoft.EntityFrameworkCore;


namespace Danplanner.Services
{

    // bruges ikke i øjeblikket
    public class CampingSiteDataService(AppDbContext db) : ICampingSiteDataService
    {
        private readonly AppDbContext _db = db;

        public async Task<List<CampingSiteDto>> GetAllAsync() =>
            await _db.CampingSites
                .Select(x => new CampingSiteDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Region = x.Region,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    HasWifi = x.HasWifi,
                    HasPlayground = x.HasPlayground,
                    HasBeach = x.HasBeach
                })
                .ToListAsync();

        public async Task<CampingSiteDto?> GetByIdAsync(int id) =>
            await _db.CampingSites
                .Where(x => x.Id == id)
                .Select(x => new CampingSiteDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Region = x.Region,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    HasWifi = x.HasWifi,
                    HasPlayground = x.HasPlayground,
                    HasBeach = x.HasBeach
                })
                .FirstOrDefaultAsync();

        public async Task<CampingSiteDto> CreateAsync(CampingSiteDto dto)
        {
            var e = new CampingSite
            {
                Name = dto.Name,
                Region = dto.Region,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
                HasWifi = dto.HasWifi,
                HasPlayground = dto.HasPlayground,
                HasBeach = dto.HasBeach
            };
            _db.CampingSites.Add(e);
            await _db.SaveChangesAsync();
            dto.Id = e.Id;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, CampingSiteDto dto)
        {
            var e = await _db.CampingSites.FindAsync(id);
            if (e is null) return false;
            e.Name = dto.Name; e.Region = dto.Region;
            e.Latitude = dto.Latitude; e.Longitude = dto.Longitude;
            e.HasWifi = dto.HasWifi; e.HasPlayground = dto.HasPlayground; e.HasBeach = dto.HasBeach;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var e = await _db.CampingSites.FindAsync(id);
            if (e is null) return false;
            _db.CampingSites.Remove(e);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
