namespace Danplanner.Services
{
    public interface IEmailService
    {
        Task SendAsync(string toEmail, string subject, string body, byte[]? attachment = null, string attachmentName = "kvittering.pdf");
    }

}
