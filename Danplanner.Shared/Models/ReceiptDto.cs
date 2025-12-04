using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danplanner.Shared.Models
{
    public class ReceiptDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
     

    }
}
