using Danplanner.Shared.Models;

namespace Danplanner.Client.Services
{
    public interface IBookingValidationService
    {
        /// <summary>
        /// Tjekker om antal personer overskrider kapacitet for valgt hytte/græsplads.
        /// Returnerer fejltekst eller tom streng hvis OK.
        /// </summary>
        string ValidateNumberOfPeople(
            BookingDto dto,
            ProductDto? selectedProduct,
            List<CottageDto> cottages,
            List<GrassFieldDto> grassFields);

        /// <summary>
        /// Tjekker om den valgte periode overlapper eksisterende bookinger
        /// for den valgte hytte/græsplads.
        /// Returnerer fejltekst eller tom streng hvis OK.
        /// </summary>
        string ValidateAvailability(
            BookingDto dto,
            ProductDto? selectedProduct,
            List<BookingDto> existingBookings);
    }
}
