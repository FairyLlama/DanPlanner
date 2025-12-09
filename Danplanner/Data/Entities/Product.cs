using Danplanner.Shared.Models;
namespace Danplanner.Data.Entities
{
    // Klasse der repræsenterer et produkt i systemet
    public class Product
    {
        public int Id { get; set; }

        // Enum i stedet for string (vi genbruger ProductType fra Shared.Models)
        public ProductType ProductType { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}   