namespace BlazorApp3semesterEksamensProjektMappe.Data.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public Resource Resource { get; set; } = null!;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
