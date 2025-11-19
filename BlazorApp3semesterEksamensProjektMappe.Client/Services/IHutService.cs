using BlazorApp3semesterEksamensProjektMappe.shared.Models;

namespace BlazorApp3semesterEksamensProjektMappe.Client.Services
{
    public interface IHutService
    {
        Task<List<HutDto>> GetAllAsync();
        Task<HutDto?> GetByIdAsync(int id);
    }

}
