using System.Net;

using Auth.Min.API.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Auth.Min.API.Services;

public class EmailService(IConfiguration configuration, ILogger<EmailService> logger) : IEmailConfigService
{
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<EmailService> _logger = logger;

    public async Task<(SmtpClient, string)> EmailSetup()
    {
        try
        {
            _logger.LogInformation("Getting email settings from appsettings.json");
            var emailSettings = _configuration.GetSection("EmailSettings") ?? throw new Exception("Email settings not found");

            _logger.LogInformation("Creating SMTP client");

            var fromEmail = emailSettings["Username"] ?? throw new Exception("From email not found");
            var client = new SmtpClient();
            await client.ConnectAsync(emailSettings["Host"], int.Parse(emailSettings["Port"] ?? "587"), SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(fromEmail, emailSettings["Password"]);

           _logger.LogInformation("Email service is configured and ready to use.");
           return (client, fromEmail);
        }
        catch (Exception ex)
        {
            throw new Exception("Error occurred while setting up email service.: " + ex.Message);
        }
    }
}
