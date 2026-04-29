using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MyBudgetApp.API.Options;

namespace MyBudgetApp.API.Services.Access;

public class EmailService
{
    private readonly EmailOptions _options;
    private readonly MailboxAddress _fromAddress;

    public EmailService(IOptions<EmailOptions> options)
    {
        _options = options.Value;
        _fromAddress = new MailboxAddress(
            _options.FromName, _options.FromEmail);
    }

    public async Task SendEmailAsync(
        string recipientAddress, string subject, string body)
    {
        using var message = new MimeMessage();
        message.From.Add(_fromAddress);
        message.To.Add(new MailboxAddress(string.Empty, recipientAddress));
        message.Subject = subject;
        message.Body = new TextPart("plain")
        {
            Text = body
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(
            _options.Host, _options.Port, _options.UseSSL);
        await client.AuthenticateAsync(_options.Username, _options.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
