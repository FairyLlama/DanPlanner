using Danplanner.Shared.Models;
using System.Net.Http.Json;

namespace Danplanner.Client.Services
{
    public class HutService : IHutService
    {
        private readonly HttpClient _http;
        public HutService(IHttpClientFactory factory) => _http = factory.CreateClient("EF");

        public async Task<List<HutDto>> GetAllAsync() =>
            await _http.GetFromJsonAsync<List<HutDto>>("api/resource") ?? new();

        public async Task<HutDto?> GetByIdAsync(int id) =>
            await _http.GetFromJsonAsync<HutDto>($"api/resource/{id}");
    }

}
