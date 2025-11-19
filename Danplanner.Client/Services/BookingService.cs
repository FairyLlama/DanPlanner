using System.Net.Http.Json;
using Danplanner.Shared.Models;

namespace Danplanner.Client.Services
{
    public class BookingService : IBookingService
    {
        private readonly HttpClient _http;

        public BookingService(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("EF");
        }

        public async Task<BookingDto?> CreateAsync(BookingDto dto)
        {
            var resp = await _http.PostAsJsonAsync("api/booking", dto);
            return resp.IsSuccessStatusCode
                ? await resp.Content.ReadFromJsonAsync<BookingDto>()
                : null;
        }

        public async Task<List<BookingDto>> GetAllAsync() =>
            await _http.GetFromJsonAsync<List<BookingDto>>("api/booking") ?? new();

        public async Task<BookingDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<BookingDto>($"api/booking/{id}");
        }
    }


}
