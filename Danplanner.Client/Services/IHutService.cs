using Danplanner.Shared.Models;

namespace Danplanner.Client.Services
{
    public interface IHutService
    {
        Task<List<HutDto>> GetAllAsync();
        Task<HutDto?> GetByIdAsync(int id);
    }

}
