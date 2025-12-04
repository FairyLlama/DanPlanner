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
        private readonly IReceiptService _receiptService;
        private readonly IEmailService _emailService;
        private readonly ILogger<BookingController> _logger;   // 👈 Logger

        public BookingController(
            IBookingDataService svc,
            IReceiptService receiptService,
            IEmailService emailService,
            ILogger<BookingController> logger)   // 👈 Logger injiceres
        {
            _svc = svc;
            _receiptService = receiptService;
            _emailService = emailService;
            _logger = logger;
        }

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

        [HttpPut("{id}/confirm")]
        public async Task<IActionResult> Confirm(int id, [FromBody] int userId)
        {
            var booking = await _svc.GetByIdAsync(id);
            if (booking == null)
                return NotFound();

            var success = await _svc.ConfirmAsync(id, userId);
            if (!success)
                return BadRequest("Kunne ikke bekræfte booking");

            try
            {
                // 🔑 Generér kvittering baseret på BookingDto
                var pdfBytes = _receiptService.GenerateReceipt(booking);

                var recipient = booking.User?.Email;
                if (string.IsNullOrWhiteSpace(recipient))
                {
                    _logger.LogWarning("Ingen emailadresse på booking {BookingId}", booking.Id);
                    return BadRequest("Brugerens email mangler. Kan ikke sende kvittering.");
                }

                _logger.LogInformation("Sender mail til {Recipient}", recipient);

                await _emailService.SendAsync(
                    recipient,
                    "Velkommen til Danplanner – din kvittering",
                    $"Hej {booking.User?.Name ?? "kunde"}! Tak for din booking af {booking.Product?.ProductType.ToString() ?? "produkt"}. Se vedhæftet kvittering.",
                    pdfBytes,
                    "kvittering.pdf"
                );

                _logger.LogInformation("Mail sendt til {Recipient}", recipient);
                return Ok("Booking bekræftet og kvittering sendt!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl under mail‑sending for booking {BookingId}", id);
                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Fejl under mail‑sending"
                );
            }
        }
    }
}