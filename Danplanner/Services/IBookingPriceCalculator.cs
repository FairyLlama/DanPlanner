using Danplanner.Shared.Models;

public interface IBookingPriceCalculator
{
    Task<decimal> CalculateTotalPriceAsync(BookingDto dto);
}
