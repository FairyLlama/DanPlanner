using Danplanner.Shared.Models;
using System.Net.Http.Json;

namespace Danplanner.Client.Services
{
    public class CottageService(IHttpClientFactory factory) : ICottageService
    {
        private readonly HttpClient _http = factory.CreateClient("EF");

        public async Task<List<CottageDto>> GetAllAsync() =>
            await _http.GetFromJsonAsync<List<CottageDto>>("api/cottage") ?? new();

        public async Task<CottageDto?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<CottageDto>($"api/cottage/{id}");


        public async Task<CottageDto?> CreateAsync(CottageDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/cottage", dto);

            var raw = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Status: {response.StatusCode}, Body: {raw}");

            response.EnsureSuccessStatusCode(); // kaster exception hvis ikke 2xx
            return await response.Content.ReadFromJsonAsync<CottageDto>();
        }

        public async Task UpdateAsync(int id, CottageDto dto)
        {
            await _http.PutAsJsonAsync($"api/cottage/{id}", dto);
        }

        public async Task DeleteAsync(int id)
        {
            await _http.DeleteAsync($"api/cottage/{id}");
        }
    }

}
