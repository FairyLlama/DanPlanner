using Danplanner.Shared.Models;

namespace Danplanner.Client.Services
{
    public interface IAddonService
    {
        Task<List<AddonDto>> GetAllAsync();
        Task<AddonDto?> GetByIdAsync(int id);
        Task<AddonDto?> CreateAsync(AddonDto dto);
        Task UpdateAsync(int id, AddonDto dto);
        Task DeleteAsync(int id);
    }
}