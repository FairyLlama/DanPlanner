using BlazorApp3semesterEksamensProjektMappe.shared.Models;

namespace BlazorApp3semesterEksamensProjektMappe.Services
{
    public interface ICampingSiteDataService
    {
        Task<List<CampingSiteDto>> GetAllAsync();
        Task<CampingSiteDto?> GetByIdAsync(int id);
        Task<CampingSiteDto> CreateAsync(CampingSiteDto dto);
        Task<bool> UpdateAsync(int id, CampingSiteDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
