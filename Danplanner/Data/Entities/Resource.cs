namespace Danplanner.Data.Entities
{
    public class Resource

    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public required string Location { get; set; }
    }

}
