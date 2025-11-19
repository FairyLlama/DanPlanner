using Danplanner.Services;
using Danplanner.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Danplanner.Controllers
{
    [ApiController]
    [Route("api/resource")]
    public class ResourceController : ControllerBase
    {
        private readonly IHutDataService _data;
        public ResourceController(IHutDataService data) => _data = data;

        [HttpGet]
        public async Task<ActionResult<List<HutDto>>> GetAll() => await _data.GetAllAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<HutDto>> GetById(int id)
        {
            var r = await _data.GetByIdAsync(id);
            return r is null ? NotFound() : r;
        }
    }

}
