
using System.Net.Mail;
using System.Net;

namespace Frieght.Api.Infrastructure.Notifications;

public class MessageSender : IMessageSender
{
    private readonly IConfiguration configuration;
    private readonly ILogger<MessageSender> logger;


    public MessageSender(IConfiguration configuration, ILogger<MessageSender> logger)
    {

        this.configuration = configuration;
        this.logger = logger;


    }

    public async Task SendEmailAsync(string recipient, string subject, string body)
    {
        var emailSettings = this.configuration.GetSection("EmailSettings") ?? throw new Exception("Email settings not found");
        string smtpServer = emailSettings["Host"] ?? throw new Exception("Mail host was not found");
        int port = int.Parse(emailSettings["Port"] ?? "587");//587; // Use the appropriate port for your SMTP server
        string userName = emailSettings["Username"] ?? throw new Exception("Username was not found");
        string senderPassword = emailSettings["Password"] ?? throw new Exception("Password was not found");
        string senderEmail = "no-reply@afroinnovate.com";

        using (var smtpClient = new SmtpClient(smtpServer, port))
        {
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(userName, senderPassword);
            smtpClient.EnableSsl = true;

            //using (var mailMessage = new MailMessage(senderEmail, recipient, subject, body))
            using (var mailMessage = new MailMessage(senderEmail, recipient, subject, body))
            {
                mailMessage.IsBodyHtml = true;
                try
                {
                    await smtpClient.SendMailAsync(mailMessage);
                    logger.LogInformation("Email sent successfully to {recipientEmail}", recipient);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error sending email: {ex.Message}");
                }
            }
        }
    }

    public Task SendSmsAsync(string toPhoneNumber, string messageBody)
    {
        throw new NotImplementedException();
    }
}
