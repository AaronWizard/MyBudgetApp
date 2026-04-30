using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MyBudgetApp.API.Options;

namespace MyBudgetApp.API.Services.Access;

public class EmailService
{
    private const string TemplatesFolder = "Templates";

    private readonly EmailOptions _options;
    private readonly IWebHostEnvironment _webHostEnvironment;

    private readonly MailboxAddress _fromAddress;

    public EmailService(
        IOptions<EmailOptions> options,
        IWebHostEnvironment webHostEnvironment
    )
    {
        _options = options.Value;
        _webHostEnvironment = webHostEnvironment;

        _fromAddress = new MailboxAddress(
            _options.FromName, _options.FromEmail);
    }

    public async Task SendEmailAsync(
        string recipientAddress, string subject, string htmlBody)
    {
        var builder = new BodyBuilder
        {
            HtmlBody = htmlBody
        };

        using var message = new MimeMessage();
        message.From.Add(_fromAddress);
        message.To.Add(new MailboxAddress(string.Empty, recipientAddress));
        message.Subject = subject;
        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        await client.ConnectAsync(
            _options.Host, _options.Port, _options.UseSSL);
        await client.AuthenticateAsync(_options.Username, _options.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task<string> GetEmailBodyFromTemplateAsync(
        string templateFile)
    {
        var templatePath = Path.Combine(
            _webHostEnvironment.ContentRootPath, TemplatesFolder, templateFile);
        return await File.ReadAllTextAsync(templatePath);
    }
}
