using Danplanner.Shared.Models;

namespace Danplanner.Services
{
    public interface IResourceDataService
    {
        Task<List<ResourceDto>> GetAllAsync();
        Task<ResourceDto?> GetByIdAsync(int id);
    }

}
