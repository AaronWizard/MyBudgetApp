using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MyBudgetApp.API.Models;
using MyBudgetApp.API.Options;

namespace MyBudgetApp.API.Services.Access;

public class RegistrationService
{
    private static class VerifyEmailConstants
    {
        public const string Subject = "My Budget App - Verify Your Account";

        public const string Template = "verify-email-template.html";
        public const string UrlToken = "{url}";
        public const string PathToken = "{path}";
        public const string TokenToken = "{token}";
    }

    private readonly UserManager<User> _userManager;
    private readonly EmailService _emailService;
    private readonly FrontEndOptions _frontEndOptions;

    public RegistrationService(
        UserManager<User> userManager,
        EmailService emailService,
        IOptions<FrontEndOptions> frontEndOptions)
    {
        _userManager = userManager;
        _emailService = emailService;
        _frontEndOptions = frontEndOptions.Value;
    }

    public async Task<bool> RegisterUserAsync(string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return false;
        }

        var user = new User
        {
            UserName = email, // Required by Identity framework
            Email = email,
            CreateDateUTC = DateTime.UtcNow
        };
        var createResult = await _userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            return false;
        }

        // Send registration email

        var registrationToken
            = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(
            Encoding.UTF8.GetBytes(registrationToken));

        var htmlBody = await GetRegistrationHTMLBodyAsync(encodedToken);

        await _emailService.SendEmailAsync(
            email, VerifyEmailConstants.Subject, htmlBody
        );

        return true;
    }

    public async Task<bool> VerifyRegistrationAsync(
        string email, string encodedToken)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return false;
        }
        if (await _userManager.IsEmailConfirmedAsync(user))
        {
            return false;
        }

        var registrationToken = Encoding.UTF8.GetString(
            WebEncoders.Base64UrlDecode(encodedToken));

        var confirmResult = await _userManager.ConfirmEmailAsync(
            user, registrationToken);
        if (!confirmResult.Succeeded)
        {
            return false;
        }

        return true;
    }

    private async Task<string> GetRegistrationHTMLBodyAsync(string token)
    {
        string htmlBody = await _emailService.GetEmailBodyFromTemplateAsync(
            VerifyEmailConstants.Template);
        htmlBody = htmlBody
            .Replace(VerifyEmailConstants.UrlToken, _frontEndOptions.BaseUrl)
            .Replace(
                VerifyEmailConstants.PathToken,
                _frontEndOptions.VerifyRegistrationPath
            )
            .Replace(VerifyEmailConstants.TokenToken, token);
        return htmlBody;
    }
}
