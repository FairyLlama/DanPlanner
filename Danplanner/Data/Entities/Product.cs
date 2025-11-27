using Danplanner.Shared.Models;
namespace Danplanner.Data.Entities
{
  

    public class Product
    {
        public int Id { get; set; }

        // Enum i stedet for string
        public ProductType ProductType { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}   