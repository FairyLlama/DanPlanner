using Danplanner.Services;
using Danplanner.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Danplanner.Controllers
{
    [ApiController]
    [Route("api/booking")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingDataService _svc;
        public BookingController(IBookingDataService svc) => _svc = svc;

        [HttpGet]
        public async Task<ActionResult<List<BookingDto>>> GetAll() =>
            await _svc.GetAllAsync();

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookingDto>> GetById(int id)
        {
            var b = await _svc.GetByIdAsync(id);
            return b is null ? NotFound() : b;
        }

        [HttpPost]
        public async Task<ActionResult<BookingDto>> Create([FromBody] BookingDto dto)
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
    }

}
