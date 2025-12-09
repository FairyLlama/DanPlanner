namespace Danplanner.Data.Entities
{

    // Klasse der repræsenterer en kvittering i systemet (obs: ikke brugt endnu)
    public class Receipt
    {
        public int Id { get; set; }
        public int BookingId { get; set; }

        public required Booking Booking { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalPrice { get; set; }
   

    }
}
