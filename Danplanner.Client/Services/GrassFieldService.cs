using Danplanner.Shared.Models;
using System.Net.Http.Json;

namespace Danplanner.Client.Services
{
    public class GrassFieldService : IGrassFieldService
    {
        private readonly HttpClient _http;

        public GrassFieldService(IHttpClientFactory factory)
        {
            // Use your existing named client that points to EF API/Gateway
            _http = factory.CreateClient("EF");
        }

        public async Task<List<GrassFieldDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<GrassFieldDto>>("api/grassfields")
                   ?? new List<GrassFieldDto>();
        }

        public async Task<GrassFieldDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<GrassFieldDto>($"api/grassfields/{id}");
        }
    }
}