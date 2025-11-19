using Danplanner.Shared.Models;

namespace Danplanner.Services
{
    public interface IResourceDataService
    {
        Task<List<HutDto>> GetAllAsync();
        Task<HutDto?> GetByIdAsync(int id);
    }

}
