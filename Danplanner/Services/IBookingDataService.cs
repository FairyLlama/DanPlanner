    using Danplanner.Shared.Models;

    namespace Danplanner.Services
    {
        public interface IBookingDataService
        {
            Task<BookingDto?> CreateAsync(BookingDto dto);
            Task<List<BookingDto>> GetAllAsync();

            Task<BookingDto?> GetByIdAsync(int id);

            // 👇 Ny metode til at bekræfte en booking
            Task<bool> ConfirmAsync(int bookingId, int userId);

        // 👇 Ny metode til at opdatere en booking
             Task<BookingDto?> UpdateAsync(BookingDto dto);


    }

}
