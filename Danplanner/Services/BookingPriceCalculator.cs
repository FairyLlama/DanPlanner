using Danplanner.Data;
using Danplanner.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Danplanner.Services
{
    public class BookingPriceCalculator : IBookingPriceCalculator
    {
        private readonly AppDbContext _db;

        public BookingPriceCalculator(AppDbContext db)
        {
            _db = db;
        }

        public async Task<decimal> CalculateTotalPriceAsync(BookingDto dto)
        {
            var days = (dto.EndDate - dto.StartDate).Days;
            if (days <= 0) days = 1;

            decimal basePrice = 0m;
            if (dto.CottageId.HasValue)
            {
                var cottage = await _db.Cottages.FindAsync(dto.CottageId.Value)
                              ?? throw new InvalidOperationException("Cottage findes ikke");
                basePrice = cottage.PricePerNight * days;
            }
            else if (dto.GrassFieldId.HasValue)
            {
                var grass = await _db.GrassFields.FindAsync(dto.GrassFieldId.Value)
                             ?? throw new InvalidOperationException("GrassField findes ikke");
                basePrice = grass.PricePerNight * days;
            }

            decimal addonsPrice = 0m;
            if (dto.BookingAddons != null)
            {
                foreach (var a in dto.BookingAddons)
                {
                    var addon = await _db.Addons.FindAsync(a.AddonId)
                                  ?? throw new InvalidOperationException("Addon findes ikke");
                    addonsPrice += addon.Price * a.Quantity;
                }
            }

            // Rabatter
            if (!string.IsNullOrEmpty(dto.DiscountType))
            {
                if (dto.DiscountType.Equals("senior", StringComparison.OrdinalIgnoreCase))
                    basePrice *= 0.85m;
                else if (dto.DiscountType.Equals("student", StringComparison.OrdinalIgnoreCase))
                    basePrice *= 0.90m;
            }

            return basePrice + addonsPrice;
        }
    }
}
