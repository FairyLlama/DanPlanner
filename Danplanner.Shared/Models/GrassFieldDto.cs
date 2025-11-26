using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danplanner.Shared.Models
{
    public class GrassFieldDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; } = "";
        public int Number { get; set; }

        public ProductDto? Product { get; set; }


    }

}
