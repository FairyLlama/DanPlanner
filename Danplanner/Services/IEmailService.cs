namespace Danplanner.Services
{
    public interface IEmailService
    {
        // Sender en email med valgfrit vedhæftet fil
        Task SendAsync(string toEmail, string subject, string body, byte[]? attachment = null, string attachmentName = "kvittering.pdf");
    }

}
