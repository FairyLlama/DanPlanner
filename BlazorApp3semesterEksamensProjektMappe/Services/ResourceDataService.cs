using BlazorApp3semesterEksamensProjektMappe.Data;
using BlazorApp3semesterEksamensProjektMappe.shared.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp3semesterEksamensProjektMappe.Services
{
    public class ResourceDataService : IResourceDataService
    {
        private readonly AppDbContext _db;
        public ResourceDataService(AppDbContext db) => _db = db;

        public async Task<List<ResourceDto>> GetAllAsync() =>
            await _db.Resources.Select(r => new ResourceDto
            {
                Id = r.Id,
                Name = r.Name,
                Type = r.Type,
                Location = r.Location
            }).ToListAsync();

        public async Task<ResourceDto?> GetByIdAsync(int id)
        {
            var r = await _db.Resources.FindAsync(id);
            return r is null ? null : new ResourceDto
            {
                Id = r.Id,
                Name = r.Name,
                Type = r.Type,
                Location = r.Location
            };
        }
    }

}
