using Danplanner.Shared.Models;

namespace Danplanner.Services
{
    public interface IGrassFieldDataService
    {
        Task<List<GrassFieldDto>> GetAllAsync();
        Task<GrassFieldDto?> GetByIdAsync(int id);
    }
}