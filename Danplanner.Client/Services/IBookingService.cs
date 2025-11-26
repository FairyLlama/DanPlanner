using Danplanner.Shared.Models;

namespace Danplanner.Client.Services
{
    public interface IBookingService
    {
        Task<List<BookingDto>> GetAllAsync();
        Task<BookingDto?> GetByIdAsync(int id);
        Task<BookingDto> CreateAsync(BookingDto dto);

        // Ny metode til at bekræfte en booking
        Task<bool> ConfirmAsync(int bookingId, int userId);

    }


}
