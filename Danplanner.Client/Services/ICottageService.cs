using Danplanner.Shared.Models;

namespace Danplanner.Client.Services
{
    public interface ICottageService
    {
        Task<List<CottageDto>> GetAllAsync();
        Task<CottageDto?> GetByIdAsync(int id);

        Task<CottageDto?> CreateAsync(CottageDto dto);

        Task UpdateAsync(int id, CottageDto dto);

        Task DeleteAsync(int id);
    }

}
