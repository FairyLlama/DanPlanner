namespace Danplanner.Data.Entities
{

    // Klasse der repræsenterer en hytte (cottage) i systemet
    public class Cottage

    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public required Product Product { get; set; }
        public int MaxCapacity { get; set; }
        public int Number { get; set; }

        public decimal PricePerNight { get; set; }

    }

}
