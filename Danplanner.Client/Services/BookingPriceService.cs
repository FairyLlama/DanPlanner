using Danplanner.Shared.Models;

namespace Danplanner.Client.Services
{
    public class BookingPriceService : IBookingPriceService
    {
        // Bruges til live-preview på AdminBooking
        public decimal CalculatePreviewTotal(
            BookingDto dto,
            ProductDto? selectedProduct,
            IEnumerable<CottageDto> cottages,
            IEnumerable<GrassFieldDto> grassFields,
            IEnumerable<AddonDto> addons,
            IReadOnlyDictionary<int, int> addonQuantities,
            IEnumerable<int> selectedAddonIds)
        {
            // 1) Find base price per nat ud fra valgt produkt
            decimal basePricePerNight = 0m;

            if (selectedProduct?.ProductType == ProductType.Cottage && dto.CottageId != null)
            {
                var cottage = cottages.FirstOrDefault(c => c.Id == dto.CottageId.Value);
                if (cottage != null)
                {
                    basePricePerNight = cottage.PricePerNight;
                }
            }
            else if (selectedProduct?.ProductType == ProductType.GrassField && dto.GrassFieldId != null)
            {
                var grass = grassFields.FirstOrDefault(g => g.Id == dto.GrassFieldId.Value);
                if (grass != null)
                {
                    basePricePerNight = grass.PricePerNight;
                }
            }

            // 2) Antal nætter
            var nights = (dto.EndDate - dto.StartDate).Days;
            if (nights < 1) nights = 1;

            // Basepris før/efter rabat
            decimal basePrice = basePricePerNight * nights;

            // 3) Addons
            decimal addonsPrice = 0m;

            foreach (var id in selectedAddonIds)
            {
                var addon = addons.FirstOrDefault(a => a.Id == id);
                if (addon is null) continue;

                var quantity = addonQuantities.TryGetValue(id, out var q) ? Math.Max(q, 1) : 1;
                addonsPrice += addon.Price * quantity;
            }

            // 4) Rabat – samme som på serveren: KUN på basePrice
            if (!string.IsNullOrWhiteSpace(dto.DiscountType))
            {
                var type = dto.DiscountType.ToLowerInvariant();
                if (type == "senior")
                {
                    basePrice *= 0.85m; // 15 % rabat
                }
                else if (type == "student")
                {
                    basePrice *= 0.90m; // 10 % rabat
                }
            }

            return basePrice + addonsPrice;
        }

        // Bruges til eksisterende bookinger (AdminGetBookings osv.)
        public decimal CalculateStoredBookingTotal(BookingDto booking)
        {
            // Her stoler vi på, at serveren allerede har regnet korrekt pris (inkl. rabat)
            // så vi viser bare det, der ligger i databasen.
            if (booking.TotalPrice > 0)
                return booking.TotalPrice;

            // Fallback hvis TotalPrice mod forventning er 0 – kan være nyttigt under udvikling
            decimal basePricePerNight = 0m;

            if (booking.Cottage != null)
            {
                basePricePerNight = booking.Cottage.PricePerNight;
            }
            else if (booking.GrassField != null)
            {
                basePricePerNight = booking.GrassField.PricePerNight;
            }

            var nights = (booking.EndDate - booking.StartDate).Days;
            if (nights < 1) nights = 1;

            decimal basePrice = basePricePerNight * nights;

            decimal addonsPrice = 0m;
            if (booking.BookingAddons != null && booking.BookingAddons.Any() && booking.Addons != null)
            {
                foreach (var ba in booking.BookingAddons)
                {
                    var addon = booking.Addons.FirstOrDefault(a => a.Id == ba.AddonId);
                    if (addon != null)
                    {
                        addonsPrice += addon.Price * ba.Quantity;
                    }
                }
            }

            return basePrice + addonsPrice;
        }
    }
}
