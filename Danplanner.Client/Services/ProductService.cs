using Danplanner.Shared.Models;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Danplanner.Client.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _jsonOptions;

        public ProductService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("EF");

            // 👇 Tilføj enum-konverteren her
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _jsonOptions.Converters.Add(new JsonStringEnumConverter());
        }

        public async Task<List<ProductDto>> GetAllAsync()
        {
            var response = await _http.GetAsync("api/product");
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<List<ProductDto>>(stream, _jsonOptions)
                   ?? new List<ProductDto>();
        }

        public async Task<ProductDto?> GetByIdAsync(int id)
        {
            var response = await _http.GetAsync($"api/product/{id}");
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<ProductDto>(stream, _jsonOptions);
        }
    }
}