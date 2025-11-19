// using Danplanner.Shared.Models;
// using System.Net.Http.Json;

// namespace Danplanner.Client.Services
// {
//     public class ResourceService : IResourceService
//     {
//         private readonly HttpClient _http;
//         public ResourceService(IHttpClientFactory factory) => _http = factory.CreateClient("EF");

//         public async Task<List<ResourceDto>> GetAllAsync() =>
//             await _http.GetFromJsonAsync<List<ResourceDto>>("api/resource") ?? new();

//         public async Task<ResourceDto?> GetByIdAsync(int id) =>
//             await _http.GetFromJsonAsync<ResourceDto>($"api/resource/{id}");
//     }

// }
