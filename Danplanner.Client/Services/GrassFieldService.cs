using Danplanner.Shared.Models;
using System.Net.Http.Json;

namespace Danplanner.Client.Services
{
    public class GrassFieldService(IHttpClientFactory factory) : IGrassFieldService
    {
        private readonly HttpClient _http = factory.CreateClient("EF");

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