using BlazorApp3semesterEksamensProjektMappe.shared.Models;

namespace BlazorApp3semesterEksamensProjektMappe.Client.Services
{
    public interface IBookingService
    {
        Task<List<BookingDto>> GetAllAsync();
        Task<BookingDto?> GetByIdAsync(int id);
        Task<BookingDto> CreateAsync(BookingDto dto);
    }


}
