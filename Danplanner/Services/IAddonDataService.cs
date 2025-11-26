using Danplanner.Shared.Models;

namespace Danplanner.Services
{
    public interface IAddonDataService
    {
        Task<List<AddonDto>> GetAllAsync();
        Task<AddonDto?> GetByIdAsync(int id);
        Task<AddonDto> CreateAsync(AddonDto dto);
        Task UpdateAsync(int id, AddonDto dto);
        Task DeleteAsync(int id);
    }
}