using Danplanner.Services;
using Danplanner.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Danplanner.Controllers
{
    [ApiController]
    [Route("api/booking")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingDataService _svc;
        private readonly IReceiptService _receiptService;
        private readonly IEmailService _emailService;
        private readonly ILogger<BookingController> _logger;
        private readonly IConfiguration _config;

        public BookingController(
            IBookingDataService svc,
            IReceiptService receiptService,
            IEmailService emailService,
            ILogger<BookingController> logger,
            IConfiguration config)
        {
            _svc = svc;
            _receiptService = receiptService;
            _emailService = emailService;
            _logger = logger;
            _config = config;
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


        [HttpPut("{id:int}")]
        public async Task<ActionResult<BookingDto>> Update(int id, [FromBody] BookingDto dto)
        {
            if (id != dto.Id)
                dto.Id = id; // sikrer at ID matcher route

            var existing = await _svc.GetByIdAsync(id);
            if (existing == null)
                return NotFound();

            var updated = await _svc.UpdateAsync(dto);
            if (updated == null)
                return BadRequest("Kunne ikke opdatere booking");

            return Ok(updated);
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
                // ⭐ 1) Tjek om vi allerede har User-data og Addon-data på booking
                bool needsDbUser = booking.User == null || string.IsNullOrWhiteSpace(booking.User.Email);
                bool needsDbAddons = booking.BookingAddons != null &&
                                     booking.BookingAddons.Any(ba => ba.Addon == null);

                string? recipient = booking.User?.Email;
                string? name = booking.User?.Name;
                string? address = booking.User?.Address;
                string? phone = booking.User?.Phone;
                string? country = booking.User?.Country;
                string? language = booking.User?.Language;

                // ⭐ 2) Hvis vi IKKE har user-info, så slå den op i DB (som før)
                if (needsDbUser)
                {
                    using (var conn = new MySqlConnection(_config.GetConnectionString("DefaultConnection")))
                    {
                        await conn.OpenAsync();
                        using var cmd = conn.CreateCommand();
                        cmd.CommandText = @"SELECT Name, Email, Address, Phone, Country, Language 
                                    FROM Users WHERE Id=@id";
                        cmd.Parameters.AddWithValue("@id", userId);

                        using var reader = await cmd.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            name = reader.GetString(0);
                            recipient = reader.GetString(1);
                            address = reader.GetString(2);
                            phone = reader.GetString(3);
                            country = reader.GetString(4);
                            language = reader.GetString(5);
                        }
                    }
                }

                // ⭐ 3) Sørg for at booking.User er sat (enten fra DB eller fra i forvejen)
                booking.User ??= new UserDto
                {
                    Id = userId,
                    Name = name ?? "",
                    Email = recipient ?? "",
                    Address = address ?? "",
                    Phone = phone ?? "",
                    Country = country ?? "",
                    Language = language ?? ""
                };

                // ⭐ 4) Hvis vi mangler Addon-data, slå dem op som før
                if (needsDbAddons && booking.BookingAddons != null)
                {
                    foreach (var ba in booking.BookingAddons)
                    {
                        using var conn = new MySqlConnection(_config.GetConnectionString("DefaultConnection"));
                        await conn.OpenAsync();
                        using var cmd = conn.CreateCommand();
                        cmd.CommandText = "SELECT Name, Price FROM Addons WHERE Id=@id";
                        cmd.Parameters.AddWithValue("@id", ba.AddonId);

                        using var reader = await cmd.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            ba.Addon = new AddonDto
                            {
                                Id = ba.AddonId,
                                Name = reader.GetString(0),
                                Price = reader.GetDecimal(1)
                            };
                        }
                    }
                }

                if (string.IsNullOrWhiteSpace(booking.User.Email))
                {
                    _logger.LogWarning("Ingen emailadresse fundet for UserId {UserId}", userId);
                    return BadRequest("Brugerens email mangler. Kan ikke sende kvittering.");
                }

                // Dansk produktnavn + nummer
                string produktTypeDa = booking.Product?.ProductType switch
                {
                    ProductType.Cottage => "hytte",
                    ProductType.GrassField => "græsplads",
                    _ => "produkt"
                };

                string produktBeskrivelse = produktTypeDa;
                if (booking.CottageId != null && booking.Cottage != null)
                    produktBeskrivelse += $" (nummer: {booking.Cottage.Number})";
                else if (booking.GrassFieldId != null && booking.GrassField != null)
                    produktBeskrivelse += $" (nummer: {booking.GrassField.Number})";

                string mailBody = $@"
Hej {booking.User?.Name}!

Tak for din booking af vores {produktBeskrivelse}.

Her er lidt praktiske informationer til din booking:
- Periode: {booking.StartDate:dd-MM-yyyy} til {booking.EndDate:dd-MM-yyyy}
- Antal personer: {booking.NumberOfPeople}
- Totalpris: {booking.TotalPrice:C}

Se vedhæftet kvittering for detaljer.
Vi glæder os til at byde dig velkommen hos Danplanner!

Adresse: Udbyhøjvej 10, 4180 Sorø
";

                // 🔑 Generér kvittering med beriget BookingDto
                var pdfBytes = _receiptService.GenerateReceipt(booking);

                _logger.LogInformation("Sender mail til {Recipient}", booking.User.Email);

                await _emailService.SendAsync(
                    booking.User.Email,
                    "Velkommen til Danplanner – din kvittering",
                    mailBody,
                    pdfBytes,
                    "kvittering.pdf"
                );

                _logger.LogInformation("Mail sendt til {Recipient}", booking.User.Email);
                return Ok("Booking bekræftet og kvittering sendt!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fejl under mail-sending for booking {BookingId}", id);
                return Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Fejl under mail-sending"
                );
            }
        }



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