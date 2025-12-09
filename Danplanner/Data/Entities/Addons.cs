namespace Danplanner.Data.Entities
{
    // Klasse der repræsenterer et tillægsprodukt (addon) i systemet
    public class Addons
    {
         public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }

        public ICollection<BookingAddon> BookingAddons { get; set; } = new List<BookingAddon>();

    }
}
