using Danplanner.Shared.Models;
using System.Net.Http.Json;

namespace Danplanner.Client.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;
        public ProductService(IHttpClientFactory factory) => _http = factory.CreateClient("EF");

        public async Task<List<ProductDto>> GetAllAsync() =>
            await _http.GetFromJsonAsync<List<ProductDto>>("api/product") ?? new();

        public async Task<ProductDto?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<ProductDto>($"api/product/{id}");
    }

}
