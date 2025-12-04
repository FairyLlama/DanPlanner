using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danplanner.Shared.Models
{
    public class BookingAddonDto
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public int AddonId { get; set; }
        public int Quantity { get; set; }

        public decimal Price { get; set; } // pris pr. enhed på tidspunktet for booking

        // Tilknyttet addon (valgfrit, men praktisk til UI)
        public AddonDto? Addon { get; set; }

    }

}
