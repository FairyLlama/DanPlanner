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


    }
}