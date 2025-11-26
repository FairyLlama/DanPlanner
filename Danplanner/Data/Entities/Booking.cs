using Danplanner.Client.Pages;
using Danplanner.Shared.Models;
namespace Danplanner.Data.Entities

{
    public class Booking
    {
        public int Id { get; set; }

        public int CampistId { get; set; }

        // FK til produkt (hytte eller græsplads)
        public int ProductId { get; set; }
        public required Product Product { get; set; }

        // FK til specifik hytte (valgfrit)
        public int? CottageId { get; set; }
        public Cottage? Cottage { get; set; }

        // FK til specifik græsplads (valgfrit)
        public int? GrassFieldId { get; set; }
        public GrassField? GrassField { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Aktiv";

        public int NumberOfPeople { get; set; } // antal personer for booking

        public ICollection<BookingAddon> BookingAddons { get; set; } = new List<BookingAddon>();

        public Receipt? Receipt { get; set; }
    }


}
