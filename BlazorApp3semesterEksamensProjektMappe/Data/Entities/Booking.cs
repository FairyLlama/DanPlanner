
using BlazorApp3semesterEksamensProjektMappe.Client.Pages;
using BlazorApp3semesterEksamensProjektMappe.shared.Models;

namespace BlazorApp3semesterEksamensProjektMappe.Data.Entities
{
    public class Booking
    {
        public int Id { get; set; }

        public int UserId { get; set; }   // Fremmednøgle til microservice
        public int ProductId { get; set; }

        public Product Product { get; set; } = null!; // Lokalt entity

        public bool CancelBooking { get; set; }
        public bool Rebook { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
