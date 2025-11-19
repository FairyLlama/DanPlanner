using Danplanner.Shared.Models;

namespace Danplanner.Services
{
    public interface IHutDataService
    {
        Task<List<HutDto>> GetAllAsync();
        Task<HutDto?> GetByIdAsync(int id);
    }

}
