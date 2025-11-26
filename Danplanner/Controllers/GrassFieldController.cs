using Danplanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace Danplanner.Controllers
{
    [ApiController]
    [Route("api/grassfields")]
    public class GrassFieldController : ControllerBase
    {
        private readonly IGrassFieldDataService _service;

        public GrassFieldController(IGrassFieldDataService service)
        {
            _service = service;
        }

        // GET: api/GrassField
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var grassFields = await _service.GetAllAsync();
            return Ok(grassFields);
        }

        // GET: api/GrassField/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var grassField = await _service.GetByIdAsync(id);
            if (grassField is null)
                return NotFound();

            return Ok(grassField);
        }
    }
}