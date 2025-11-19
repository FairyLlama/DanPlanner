using Microsoft.AspNetCore.Mvc;
using Danplanner.Services;
using Danplanner.Shared.Models;

namespace Danplanner.Controllers
{
    [ApiController]
    [Route("api/campingsites")]
    public class CampingSitesController : ControllerBase
    {
        private readonly ICampingSiteDataService _svc;
        public CampingSitesController(ICampingSiteDataService svc) => _svc = svc;

        [HttpGet] public async Task<ActionResult<IEnumerable<CampingSiteDto>>> Get() => Ok(await _svc.GetAllAsync());
        [HttpGet("{id}")]
        public async Task<ActionResult<CampingSiteDto>> GetById(int id) =>
            (await _svc.GetByIdAsync(id)) is { } site ? Ok(site) : NotFound();
        [HttpPost]
        public async Task<IActionResult> Create(CampingSiteDto dto) =>
            CreatedAtAction(nameof(GetById), new { id = (await _svc.CreateAsync(dto)).Id }, dto);
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CampingSiteDto dto) =>
            await _svc.UpdateAsync(id, dto) ? NoContent() : NotFound();
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) =>
            await _svc.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
