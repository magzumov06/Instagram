using Domain.DTOs.EmailDto;
using Infrastructure.Helper;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Options;
using Serilog;

namespace Infrastructure.Services;

public class EmailService(IOptions<EmailSetting> options) : IEmailService
{
    private readonly EmailSetting _settings = options.Value;

    public async Task SendEmail(SendEmail send)
    {
        try
        {
            Log.Information("Sending email");
            await EmailHelper.SendEmailAsync(send, _settings);
        }
        catch (Exception )
        {
            Log.Error("Error in SendEmail");
        }
    }
}