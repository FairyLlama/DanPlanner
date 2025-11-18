namespace Danplanner.Data.Entities
{
    public class CampingSite
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Region { get; set; } = "Danmark";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool HasWifi { get; set; }
        public bool HasPlayground { get; set; }
        public bool HasBeach { get; set; }
    }
}
