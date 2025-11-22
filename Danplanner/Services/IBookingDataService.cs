using Danplanner.Shared.Models;

namespace Danplanner.Services
{
    public interface IBookingDataService
    {
        Task<BookingDto?> CreateAsync(BookingDto dto);
        Task<List<BookingDto>> GetAllAsync();

        Task<BookingDto?> GetByIdAsync(int id);
    }

}
