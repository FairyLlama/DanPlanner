namespace Danplanner.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductType { get; set; } = ""; // Hytte, Græsplads
        public decimal PricePerNight { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }


}
