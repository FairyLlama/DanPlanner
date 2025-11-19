using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp3semesterEksamensProjektMappe.shared.Models
{
    public class UserDto
    {
        public int Id { get; set; }

        // Ekstra oplysninger om campisten
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }

}
