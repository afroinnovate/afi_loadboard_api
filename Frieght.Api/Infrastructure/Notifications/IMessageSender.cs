namespace Frieght.Api.Infrastructure.Notifications;

/// <summary>
/// Interface for sending emails.
/// Sends an  email asynchronously.
/// </summary>
/// <param name="toEmail">Recipient's email address.</param>
/// <param name="subject">Email subject.</param>
/// <param name="Body">body content of the email.</param>
/// <returns>A task representing the asynchronous operation.</returns>
/// </summary>

public interface IMessageSender
{
    Task SendEmailAsync(string recipient, string subject, string body);
    Task SendSmsAsync(string toPhoneNumber, string messageBody);
}
