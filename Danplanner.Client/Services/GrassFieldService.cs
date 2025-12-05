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

        public async Task<GrassFieldDto?> CreateAsync(GrassFieldDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/grassfields", dto);
            return await response.Content.ReadFromJsonAsync<GrassFieldDto>();
        }

        public async Task UpdateAsync(int id, GrassFieldDto dto)
        {
            await _http.PutAsJsonAsync($"api/grassfields/{id}", dto);
        }

        public async Task DeleteAsync(int id)
        {
            await _http.DeleteAsync($"api/grassfields/{id}");
        }
    }
}