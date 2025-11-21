using Danplanner.Shared.Models;

namespace Danplanner.Services
{
    public interface ICottageDataService
    {
        Task<List<CottageDto>> GetAllAsync();
        Task<CottageDto?> GetByIdAsync(int id);
    }

}
