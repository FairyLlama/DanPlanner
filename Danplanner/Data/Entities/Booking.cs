using Danplanner.Client.Pages;
using Danplanner.Shared.Models;

namespace Danplanner.Data.Entities


// Klasse der repræsenterer en booking i systemet
{
    public class Booking
    {
        public int Id { get; set; }

        public int CampistId { get; set; }

        public int UserId  { get; set; }

       
        public int ProductId { get; set; }
        public required Product Product { get; set; }


        public int? CottageId { get; set; }
        public Cottage? Cottage { get; set; }

     
        public int? GrassFieldId { get; set; }
        public GrassField? GrassField { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "Aktiv";

        public int NumberOfPeople { get; set; } 

        public decimal TotalPrice { get; set; } 

        public ICollection<BookingAddon> BookingAddons { get; set; } = new List<BookingAddon>();

        public Receipt? Receipt { get; set; }

        public string? DiscountType { get; set; }
    }


}
