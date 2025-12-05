using Danplanner.Services;
using Danplanner.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Danplanner.Controllers
{
    [ApiController]
    [Route("api/cottage")]
    public class CottageController : ControllerBase
    {
        private readonly ICottageDataService _svc;
        public CottageController(ICottageDataService svc) => _svc = svc;

        // GET: api/cottages
        [HttpGet]
        public async Task<ActionResult<List<CottageDto>>> GetAll() =>
            await _svc.GetAllAsync();

        // GET: api/cottages/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CottageDto>> GetById(int id)
        {
            var c = await _svc.GetByIdAsync(id);
            return c is null ? NotFound() : c;
        }

        // POST: api/cottages

        [HttpPost]
        public async
            Task<ActionResult<CottageDto>> Create(CottageDto dto)
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CottageDto>> Update(int id, CottageDto dto)
        {
            if (id != dto.Id)
                dto.Id = id; // ensure ID matches route
            var existing = await _svc.GetByIdAsync(id);
            if (existing == null)
                return NotFound();
            var updated = await _svc.UpdateAsync(id, dto);
            return Ok(updated);
        }

        // DELETE: api/cottages/5

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _svc.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

    }
}