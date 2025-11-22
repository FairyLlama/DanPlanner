using System.Net.Http.Json;
using Danplanner.Shared.Models;

namespace Danplanner.Client.Services
{
    public class CampingSitesService : ICampingSitesService
    {
        private readonly HttpClient _http;
        public CampingSitesService(IHttpClientFactory factory)
        {
            // Use your existing named client that points to EF API/Gateway
            _http = factory.CreateClient("EF");
        }

        public async Task<List<CampingSiteDto>> GetAllAsync() =>
            await _http.GetFromJsonAsync<List<CampingSiteDto>>("api/campingsites") ?? new();

        public async Task<CampingSiteDto?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<CampingSiteDto>($"api/campingsites/{id}");

        public async Task<CampingSiteDto?> CreateAsync(CampingSiteDto dto)
        {
            var resp = await _http.PostAsJsonAsync("api/campingsites", dto);
            return resp.IsSuccessStatusCode ? await resp.Content.ReadFromJsonAsync<CampingSiteDto>() : null;
        }

        public async Task<bool> UpdateAsync(int id, CampingSiteDto dto) =>
            (await _http.PutAsJsonAsync($"api/campingsites/{id}", dto)).IsSuccessStatusCode;

        public async Task<bool> DeleteAsync(int id) =>
            (await _http.DeleteAsync($"api/campingsites/{id}")).IsSuccessStatusCode;
    }
}
