using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danplanner.Shared.Models
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string ProductType { get; set; } = "";   

        public decimal SeasonalPrice { get; set; }

        public decimal ServicePrice { get; set; }

        public int NumberOfGuests { get; set; } 

        public string AdditionalPurchases { get; set; } = "";
    }

}
