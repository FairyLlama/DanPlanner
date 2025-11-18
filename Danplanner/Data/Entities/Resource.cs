namespace Danplanner.Data.Entities
{
    public class Resource

    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Type { get; set; } = ""; // fx "Plads", "Hytte"
        public string Location { get; set; } = "";
    }

}
