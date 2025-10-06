using Domain.DTOs.EmailDto;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Infrastructure.Helper;

public static class EmailHelper
{
    public static async Task SendEmailAsync(SendEmail send,
        EmailSetting setting)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(setting.FromName, setting.FromEmail));
        message.To.Add(MailboxAddress.Parse(send.To));
        message.Subject = send.Subject;
        var bodyBuilder = new BodyBuilder {HtmlBody = send.Body};
        message.Body = bodyBuilder.ToMessageBody();
        
        using var client = new SmtpClient();
        try
        {
            if (setting.UseSsl)
            {
                await client.ConnectAsync(setting.Host, setting.Port, SecureSocketOptions.SslOnConnect);
            }
            else if (setting.UseStartTls)
            {
                await client.ConnectAsync(setting.Host, setting.Port, SecureSocketOptions.StartTls);
            }
            else
            {
                await client.ConnectAsync(setting.Host, setting.Port, SecureSocketOptions.None);
            }

            if (!string.IsNullOrEmpty(setting.Username))
            {
                await client.AuthenticateAsync(setting.Username, setting.Password);
            }

            await client.SendAsync(message);
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}