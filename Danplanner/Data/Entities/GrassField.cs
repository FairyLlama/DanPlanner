namespace Danplanner.Data.Entities
{

    // Klasse der repræsenterer et græsareal i systemet
    public class GrassField
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public required Product Product { get; set; }

        public int MaxCapacity { get; set; }
        public string Size { get; set; } = "";
        public int Number { get; set; }

        public decimal PricePerNight { get; set; }

    }
}
