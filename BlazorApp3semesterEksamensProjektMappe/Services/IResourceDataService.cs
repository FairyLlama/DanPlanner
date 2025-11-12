using BlazorApp3semesterEksamensProjektMappe.shared.Models;

namespace BlazorApp3semesterEksamensProjektMappe.Services
{
    public interface IResourceDataService
    {
        Task<List<ResourceDto>> GetAllAsync();
        Task<ResourceDto?> GetByIdAsync(int id);
    }

}
