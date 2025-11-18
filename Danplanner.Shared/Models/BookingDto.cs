using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danplanner.Shared.Models
{
    public class BookingDto
    {
        public int Id { get; set; }

        // Samme navngivning som entity
        public int ResourceId { get; set; }
        public string ResourceName { get; set; } = "";

        public int ProductId { get; set; }
        public string ProductName { get; set; } = "";

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal TotalPrice { get; set; }
    }

}
