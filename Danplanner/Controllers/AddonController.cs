using Danplanner.Shared.Models;
using Danplanner.Services;
using Microsoft.AspNetCore.Mvc;

namespace Danplanner.Controllers
{
    [ApiController]
    [Route("api/addon")]
    public class AddonController : ControllerBase
    {
        private readonly IAddonDataService _service;

        public AddonController(IAddonDataService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddonDto>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddonDto>> GetById(int id)
        {
            var addon = await _service.GetByIdAsync(id);
            if (addon == null) return NotFound();
            return Ok(addon);
        }

        [HttpPost]
        public async Task<ActionResult<AddonDto>> Create(AddonDto dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, AddonDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}