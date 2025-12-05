using Danplanner.Shared.Models;

namespace Danplanner.Services
{
    public interface ICottageDataService
    {
        Task<List<CottageDto>> GetAllAsync();
        Task<CottageDto?> GetByIdAsync(int id);

        Task<CottageDto> CreateAsync(CottageDto dto);

        Task<CottageDto> UpdateAsync(int id, CottageDto dto);

        Task DeleteAsync(int id);


    }

}
