using Danplanner.Shared.Models;

namespace Danplanner.Client.Services
{
    public interface IGrassFieldService
    {
        Task<List<GrassFieldDto>> GetAllAsync();
        Task<GrassFieldDto?> GetByIdAsync(int id);
    }
}