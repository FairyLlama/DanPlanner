using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Danplanner.Shared.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        // Ekstra oplysninger om campisten
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Address { get; set; }
        public required string Phone { get; set; }
        public required string Country { get; set; }
        public required string Language { get; set; }


    }

}
