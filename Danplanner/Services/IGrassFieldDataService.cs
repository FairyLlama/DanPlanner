using Danplanner.Shared.Models;

namespace Danplanner.Services
{
    public interface IGrassFieldDataService
    {
        // CRUD operationer for GrassField entiteten
        Task<List<GrassFieldDto>> GetAllAsync();
        Task<GrassFieldDto?> GetByIdAsync(int id);

        Task<GrassFieldDto> CreateAsync(GrassFieldDto dto);
        Task<GrassFieldDto> UpdateAsync(int id, GrassFieldDto dto);

        Task DeleteAsync(int id);
    }
}