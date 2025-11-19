namespace Danplanner.Data.Entities
{
    public class Hut

    {
        public int Id { get; set; }

        public int MaxCapacity { get; set; }

        public int ProductId { get; set; }

        public required Product Product { get; set; }
    }

}
