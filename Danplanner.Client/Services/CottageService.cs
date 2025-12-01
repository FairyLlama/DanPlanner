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
    }

}
