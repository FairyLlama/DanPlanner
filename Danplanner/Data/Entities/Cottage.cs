namespace Danplanner.Data.Entities
{
    public class Cottage

    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public required Product Product { get; set; }
        public int MaxCapacity { get; set; }
        public int Number { get; set; } 

    }

}
