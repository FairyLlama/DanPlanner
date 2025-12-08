using Danplanner.Shared.Models;

namespace Danplanner.Client.Services
{
    public class BookingValidationService : IBookingValidationService
    {
        public string ValidateNumberOfPeople(
            BookingDto dto,
            ProductDto? selectedProduct,
            List<CottageDto> cottages,
            List<GrassFieldDto> grassFields)
        {
            if (selectedProduct?.ProductType == ProductType.Cottage && dto.CottageId != null)
            {
                var cottage = cottages.FirstOrDefault(c => c.Id == dto.CottageId.Value);
                if (cottage != null && dto.NumberOfPeople > cottage.MaxCapacity)
                    return $"Der kan maks være {cottage.MaxCapacity} personer i den valgte hytte.";
            }
            else if (selectedProduct?.ProductType == ProductType.GrassField && dto.GrassFieldId != null)
            {
                var grass = grassFields.FirstOrDefault(g => g.Id == dto.GrassFieldId.Value);
                if (grass != null && dto.NumberOfPeople > grass.MaxCapacity)
                    return $"Der kan maks være {grass.MaxCapacity} personer på den valgte græsplads.";
            }

            return string.Empty;
        }

        public string ValidateAvailability(
            BookingDto dto,
            ProductDto? selectedProduct,
            List<BookingDto> existingBookings)
        {
            // datoer skal være gyldige
            if (dto.EndDate <= dto.StartDate)
                return string.Empty; // den fejl håndteres et andet sted

            int? cottageId = dto.CottageId;
            int? grassFieldId = dto.GrassFieldId;

            if (cottageId == null && grassFieldId == null)
                return string.Empty;

            var relevant = existingBookings
                .Where(b =>
                    (cottageId != null && b.CottageId == cottageId) ||
                    (grassFieldId != null && b.GrassFieldId == grassFieldId))
                .Where(b =>
                    string.Equals(b.Status, "Aktiv", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(b.Status, "Afventer", StringComparison.OrdinalIgnoreCase))
                .ToList();

            foreach (var b in relevant)
            {
                if (DateRangesOverlap(dto.StartDate, dto.EndDate, b.StartDate, b.EndDate))
                {
                    return $"Den valgte periode overlapper booking #{b.Id} ({b.StartDate:dd/MM}-{b.EndDate:dd/MM}).";
                }
            }

            return string.Empty;
        }

        private static bool DateRangesOverlap(
            DateTime start1, DateTime end1,
            DateTime start2, DateTime end2)
        {
            start1 = start1.Date;
            end1 = end1.Date;
            start2 = start2.Date;
            end2 = end2.Date;

            // Overlap, hvis de deler mindst én dato
            return start1 <= end2 && start2 <= end1;
        }
    }
}
