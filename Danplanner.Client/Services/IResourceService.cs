using Danplanner.Shared.Models;

namespace Danplanner.Client.Services
{
    public interface IResourceService
    {
        Task<List<ResourceDto>> GetAllAsync();
        Task<ResourceDto?> GetByIdAsync(int id);
    }

}
