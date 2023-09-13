namespace EmailMarketingWebApi.Services
{
    using System;
    using System.Net;
    using System.Net.Mail;
    using Microsoft.Extensions.Configuration;

    public class EmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;

            // Configure the SMTP client
            _smtpClient = new SmtpClient(_configuration["SmtpSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["SmtpSettings:Port"]),
                Credentials = new NetworkCredential(_configuration["SmtpSettings:Username"], _configuration["SmtpSettings:Password"]),
                EnableSsl = true // Set to true for TLS, or false for no encryption
            };
        }

        public void SendEmail(string to, string subject, string body)
        {
            var from = _configuration["SmtpSettings:Username"]; // Use the same email as the SMTP username

            using (var mailMessage = new MailMessage(from, to)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true // Set to true if the body contains HTML
            })
            {
                try
                {
                    _smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    // Handle exceptions (e.g., logging)
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }

}
