using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Danplanner.Shared.Models
{
    public enum ProductType
    {
        Cottage = 1,
        GrassField = 2
    }

    public class ProductDto
    {
        public int Id { get; set; }

        // DTO bruger sin egen enum (så du undgår afhængighed fra Entities)
        public ProductType ProductType { get; set; }


    }
}

