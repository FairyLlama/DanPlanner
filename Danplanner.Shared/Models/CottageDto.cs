using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danplanner.Shared.Models
{
    public class CottageDto
    {
        public int Id { get; set; }

        public int MaxCapacity { get; set; }

        public int ProductId { get; set; }

        public ProductDto? Product { get; set; }


    }

}
