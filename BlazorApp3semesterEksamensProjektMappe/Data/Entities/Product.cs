namespace BlazorApp3semesterEksamensProjektMappe.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string ProductType { get; set; } = "";

        public decimal SeasonalPrice { get; set; }

        public decimal ServicePrice { get; set; }

        public int NumberOfGuests { get; set; }

        public string AdditionalPurchases { get; set; } = "";
    }

}
