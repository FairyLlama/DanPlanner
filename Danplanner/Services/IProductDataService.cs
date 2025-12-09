using Danplanner.Shared.Models;

namespace Danplanner.Services
{
    public interface IProductDataService
    {

        // GET: api/product
        Task<List<ProductDto>> GetAllAsync();
        Task<ProductDto?> GetByIdAsync(int id);
    }

}
