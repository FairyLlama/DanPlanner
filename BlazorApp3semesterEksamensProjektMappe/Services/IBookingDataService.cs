using BlazorApp3semesterEksamensProjektMappe.shared.Models;

namespace BlazorApp3semesterEksamensProjektMappe.Services
{
    public interface IBookingDataService
    {
        Task<BookingDto?> CreateAsync(BookingDto dto);
        Task<List<BookingDto>> GetAllAsync();

        Task<BookingDto?> GetByIdAsync(int id);
    }

}
