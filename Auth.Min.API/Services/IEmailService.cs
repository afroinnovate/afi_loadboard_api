namespace Auth.Min.API.Services;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

public interface IEmailConfigService
{
    public Task<(SmtpClient, string)> EmailSetup();
}

