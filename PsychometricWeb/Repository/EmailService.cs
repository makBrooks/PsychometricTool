using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;

namespace PsychometricWeb.Repository
{
    public class EmailService
    {
        private readonly string _senderEmail;
        private readonly string _senderPassword;
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _senderEmail = configuration["EmailSettings:SenderEmail"];
            _senderPassword = configuration["EmailSettings:SenderPassword"];
            _smtpServer = configuration["EmailSettings:SmtpServer"];
            _smtpPort = int.Parse(configuration["EmailSettings:SmtpPort"]);
            _logger = logger;
        }

        public async Task<IActionResult> SendOtpEmailAsync(string recipientEmail, string otp)
        {
            // Create the email message
            MailMessage mailMessage = new MailMessage(_senderEmail, recipientEmail, "Your OTP", $"Your OTP is: {otp}");

            // Configure the SMTP client
            using (SmtpClient smtpClient = new SmtpClient(_smtpServer, _smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(_senderEmail, _senderPassword);
                smtpClient.EnableSsl = true; // Enable SSL for the connection

                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    _logger.LogInformation("Email sent successfully to {RecipientEmail}.", recipientEmail);
                    return new JsonResult(new { success = true });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send email to {RecipientEmail}.", recipientEmail);
                    return new JsonResult(new { success = false, error = ex.Message });
                }
            }
        }
    }
}