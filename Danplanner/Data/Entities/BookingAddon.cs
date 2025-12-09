namespace Danplanner.Data.Entities
{
    // Klasse der repræsenterer et tillægsprodukt (addon) knyttet til en booking
    public class BookingAddon
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public int AddonId { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; } 
        public Booking? Booking { get; set; } 
        public Addons? Addons { get; set; }

    }
}
