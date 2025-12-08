using Danplanner.Shared.Models;

namespace Danplanner.Client.Services
{
    public interface IBookingPriceService
    {
        decimal CalculatePreviewTotal(
            BookingDto dto,
            ProductDto? selectedProduct,
            IEnumerable<CottageDto> cottages,
            IEnumerable<GrassFieldDto> grassFields,
            IEnumerable<AddonDto> addons,
            IReadOnlyDictionary<int, int> addonQuantities,
            IEnumerable<int> selectedAddonIds);

        decimal CalculateStoredBookingTotal(BookingDto booking);
    }
}
