using Danplanner.Shared.Models;
using System.Net.Http.Json;

namespace Danplanner.Client.Services
{
    public class AddonService(IHttpClientFactory factory) : IAddonService
    {
        private readonly HttpClient _http = factory.CreateClient("EF");

        public async Task<List<AddonDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<AddonDto>>("api/addon") ?? new List<AddonDto>();
        }

        public async Task<AddonDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<AddonDto>($"api/addon/{id}");
        }

        public async Task<AddonDto?> CreateAsync(AddonDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/addon", dto);
            return await response.Content.ReadFromJsonAsync<AddonDto>();
        }

        public async Task UpdateAsync(int id, AddonDto dto)
        {
            await _http.PutAsJsonAsync($"api/addon/{id}", dto);
        }

        public async Task DeleteAsync(int id)
        {
            await _http.DeleteAsync($"api/addon/{id}");
        }
    }
}