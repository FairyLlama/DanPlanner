using BlazorApp3semesterEksamensProjektMappe.shared.Models;

namespace BlazorApp3semesterEksamensProjektMappe.Services
{
    public interface IHutDataService
    {
        Task<List<HutDto>> GetAllAsync();
        Task<HutDto?> GetByIdAsync(int id);
    }

}
