using Danplanner.Services;
using Danplanner.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Danplanner.Controllers
{

    // API controller for managing GrassFields

    [ApiController]
    [Route("api/grassfields")]
    public class GrassFieldController : ControllerBase
    {
        private readonly IGrassFieldDataService _service;

        public GrassFieldController(IGrassFieldDataService service)
        {
            _service = service;
        }

        // GET: api/grassfields
        [HttpGet]
        public async Task<ActionResult<List<GrassFieldDto>>> GetAll()
        {
            var grassFields = await _service.GetAllAsync();
            return Ok(grassFields);
        }

        // GET: api/grassfields
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GrassFieldDto>> GetById(int id)
        {
            var grassField = await _service.GetByIdAsync(id);
            if (grassField is null)
                return NotFound();

            return Ok(grassField);
        }

        // POST: api/grassfields
        [HttpPost]
        public async Task<ActionResult<GrassFieldDto>> Create(GrassFieldDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/grassfields
        [HttpPut("{id:int}")]
        public async Task<ActionResult<GrassFieldDto>> Update(int id, GrassFieldDto dto)
        {
            if (id != dto.Id)
                dto.Id = id; // sikrer at route-id matcher dto-id

            var existing = await _service.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            var updated = await _service.UpdateAsync(id, dto);
            return Ok(updated);
        }

        // DELETE: api/grassfields
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}