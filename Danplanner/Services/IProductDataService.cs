using Danplanner.Shared.Models;

namespace Danplanner.Services
{
    public interface IProductDataService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
    }

}
