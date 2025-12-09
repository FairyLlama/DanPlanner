using Danplanner.Shared.Models;

public interface IBookingPriceCalculator
{

    // Beregner den totale pris for en booking baseret på dens detaljer.
    Task<decimal> CalculateTotalPriceAsync(BookingDto dto);
}
