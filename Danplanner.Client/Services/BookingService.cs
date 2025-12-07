using Danplanner.Shared.Models;
using System.Net.Http.Json;

namespace Danplanner.Client.Services
{
    public class BookingService(IHttpClientFactory factory) : IBookingService
    {
        private readonly HttpClient _http = factory.CreateClient("EF");

        public async Task<List<BookingDto>> GetAllAsync()
        {
            return await _http.GetFromJsonAsync<List<BookingDto>>("api/booking") ?? new List<BookingDto>();
        }

        public async Task<BookingDto?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<BookingDto>($"api/booking/{id}");
        }

        public async Task<BookingDto?> CreateAsync(BookingDto dto)
        {
            var response = await _http.PostAsJsonAsync("api/booking", dto);
            return await response.Content.ReadFromJsonAsync<BookingDto>();
        }

        public async Task<bool> ConfirmAsync(int bookingId, int userId)
        {
            var response = await _http.PutAsJsonAsync($"api/booking/{bookingId}/confirm", userId);
            return response.IsSuccessStatusCode;
        }

        public async Task UpdateAsync(int id, BookingDto dto)
        {
            await _http.PutAsJsonAsync($"api/booking/{id}", dto);
        }

        public async Task DeleteAsync(int id)
        {
            await _http.DeleteAsync($"api/booking/{id}");
        }
    }
}
