using Danplanner.Services;
using Danplanner.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Danplanner.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductDataService _data;
        public ProductController(IProductDataService data) => _data = data;

        [HttpGet]
        public async Task<ActionResult<List<ProductDto>>> GetAll() => await _data.GetAllAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var p = await _data.GetByIdAsync(id);
            return p is null ? NotFound() : p;
        }
    }

}
