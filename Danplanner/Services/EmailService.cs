using System.Net;
using System.Net.Mail;

namespace Danplanner.Services
{

    // Service til at sende emails via SMTP
    public class EmailService(IConfiguration config) : IEmailService
    {
        private readonly IConfiguration _config = config;

        // Sender en email med PDF-attachment
        public async Task SendAsync(string toEmail, string subject, string body, byte[]? attachment = null, string attachmentName = "kvittering.pdf")
        {
            var settings = _config.GetSection("EmailSettings");

            using var message = new MailMessage();
            message.From = new MailAddress(settings["SenderEmail"], settings["SenderName"]);
            message.To.Add(toEmail);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = false;

            if (attachment != null)
            {
                var stream = new MemoryStream(attachment);
                message.Attachments.Add(new Attachment(stream, attachmentName, "application/pdf"));
            }

            using var client = new SmtpClient(settings["SmtpServer"], int.Parse(settings["Port"]))
            {
                Credentials = new NetworkCredential(settings["Username"], settings["Password"]),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }
    }
}
    




