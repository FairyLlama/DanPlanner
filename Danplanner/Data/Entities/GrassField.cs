namespace Danplanner.Data.Entities
{
    public class GrassField
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        public required Product Product { get; set; }

        public string Size { get; set; } = "";
        public int Number { get; set; }

        public decimal PricePerNight { get; set; }

    }
}
