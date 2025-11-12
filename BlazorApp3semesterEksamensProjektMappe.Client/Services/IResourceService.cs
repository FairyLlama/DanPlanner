using BlazorApp3semesterEksamensProjektMappe.shared.Models;

namespace BlazorApp3semesterEksamensProjektMappe.Client.Services
{
    public interface IResourceService
    {
        Task<List<ResourceDto>> GetAllAsync();
        Task<ResourceDto?> GetByIdAsync(int id);
    }

}
