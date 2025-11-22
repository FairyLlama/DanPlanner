using Danplanner.Shared.Models;

namespace Danplanner.Client.Services
{
    public interface ICottageService
    {
        Task<List<CottageDto>> GetAllAsync();
        Task<CottageDto?> GetByIdAsync(int id);
    }

}
