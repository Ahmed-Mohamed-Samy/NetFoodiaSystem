using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetFoodia.Services_Abstraction;
using System.Net;
using System.Net.Mail;

namespace NetFoodia.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettingsOptions, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettingsOptions.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                using var client = new SmtpClient(_emailSettings.Host, _emailSettings.Port)
                {
                    Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.Password),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Email successfully sent to {ToEmail}", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending email to {ToEmail}", toEmail);
                // In a production scenario, we might want to throw or handle this gracefully.
                // For now, we just log it to prevent failing the entire registration/forgot password flow
                // if SMTP is purely misconfigured locally.
            }
        }
    }
}
