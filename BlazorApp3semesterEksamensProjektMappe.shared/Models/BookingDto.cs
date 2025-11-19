using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp3semesterEksamensProjektMappe.shared.Models
{
    public class BookingDto
    {
        public int Id { get; set; }

        // Fremmednøgler
        public int UserId { get; set; }
        public int ProductId { get; set; }

        // Evt. detaljer (kun hvis nødvendigt)
        public UserDto? User { get; set; }
        public ProductDto? Product { get; set; }

        public bool CancelBooking { get; set; }
        public bool Rebook { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
