using Danplanner.Shared.Models;
using System.Net.Http.Json;

namespace Danplanner.Client.Services
{
    public class CottageService : ICottageService
    {
        private readonly HttpClient _http;
        public CottageService(IHttpClientFactory factory) => _http = factory.CreateClient("EF");

        public async Task<List<CottageDto>> GetAllAsync() =>
            await _http.GetFromJsonAsync<List<CottageDto>>("api/resource") ?? new();

        public async Task<CottageDto?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<CottageDto>($"api/resource/{id}");
    }

}
