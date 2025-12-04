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

        public int? UserId { get; set; } 
        public UserDto? User { get; set; } // valgfrit, til visning

        public int? CottageId { get; set; }   // specifik hytte
        public CottageDto? Cottage { get; set; }

        public int? GrassFieldId { get; set; }   // specifik græsplads
        public GrassFieldDto? GrassField { get; set; }

        public int ProductId { get; set; }

        public ProductDto? Product { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Status { get; set; } = "";

        public int NumberOfPeople { get; set; } // antal personer for booking

        public List<AddonDto>? Addons { get; set; }

        public ReceiptDto? Receipt { get; set; }

        public decimal TotalPrice { get; set; } // samlet pris for hele bookingen

        public List<BookingAddonDto> BookingAddons { get; set; } = new();

        

    }

}
