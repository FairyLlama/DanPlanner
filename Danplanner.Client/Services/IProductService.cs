using Danplanner.Shared.Models;

namespace Danplanner.Client.Services
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
    }

}
