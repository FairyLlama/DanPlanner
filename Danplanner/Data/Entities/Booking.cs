using Danplanner.Client.Pages;
using Danplanner.Shared.Models;
namespace Danplanner.Data.Entities

{
    public class Booking
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int ProductId { get; set; }

        public required Product Product { get; set; } 

        public bool CancelBooking { get; set; }
        public bool Rebook { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
