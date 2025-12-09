    using Danplanner.Shared.Models;

    namespace Danplanner.Services
    {
        public interface IBookingDataService
        {

        // CRUD Operations for Bookings
        Task<BookingDto?> CreateAsync(BookingDto dto);
            Task<List<BookingDto>> GetAllAsync();

            Task<BookingDto?> GetByIdAsync(int id);

            Task<bool> ConfirmAsync(int bookingId, int userId);

        
             Task<BookingDto?> UpdateAsync(BookingDto dto);


            Task DeleteAsync(int id);


    }

}
