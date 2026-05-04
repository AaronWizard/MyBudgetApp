using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MyBudgetApp.API.Models;
using MyBudgetApp.API.Options;

namespace MyBudgetApp.API.Services.Access;

public class RegistrationService
{
    public enum RegistrationResultType
    {
        Succeeded,
        UserAlreadyExists,
        InvalidEmail,
        InvalidPassword,
        InvalidEmailAndPassword,
        OtherError
    }

    public record RegistrationResult(
        RegistrationResultType Type, string[] PasswordErrors);

    public enum VerificationResultType
    {
        Success,
        UserNotFound,
        AlreadyVerified,
        InvalidToken,
        OtherError
    }

    private static class VerifyEmailConstants
    {
        public const string Subject = "My Budget App - Verify Your Account";

        public const string Template = "verify-email-template.html";
        public const string UrlToken = "{url}";
        public const string PathToken = "{path}";
        public const string UserToken = "{user}";
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

    public async Task<RegistrationResult> RegisterUserAsync(
        string email, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return new RegistrationResult(
                RegistrationResultType.UserAlreadyExists, Array.Empty<string>()
            );
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
            var invalidEmail = createResult.Errors.Any(
                e => e.Code == "InvalidEmail"
            );
            var passwordErrors = createResult.Errors
                .Where(e => e.Code.StartsWith("Password"))
                .Select(e => e.Description);

            var type = RegistrationResultType.OtherError;
            if (invalidEmail && passwordErrors.Any())
            {
                type = RegistrationResultType.InvalidEmailAndPassword;
            }
            else if (invalidEmail)
            {
                type = RegistrationResultType.InvalidEmail;
            }
            else if (passwordErrors.Any())
            {
                type = RegistrationResultType.InvalidPassword;
            }

            return new RegistrationResult(type, passwordErrors.ToArray());
        }

        // Send registration email

        var registrationToken
            = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var encodedToken = WebEncoders.Base64UrlEncode(
            Encoding.UTF8.GetBytes(registrationToken));

        var htmlBody = await GetRegistrationHTMLBodyAsync(
            user.Id, encodedToken);

        await _emailService.SendEmailAsync(
            email, VerifyEmailConstants.Subject, htmlBody
        );

        return new RegistrationResult(
            RegistrationResultType.Succeeded, Array.Empty<string>());
    }

    public async Task<VerificationResultType> VerifyRegistrationAsync(
        string userId, string encodedToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return VerificationResultType.UserNotFound;
        }
        if (await _userManager.IsEmailConfirmedAsync(user))
        {
            return VerificationResultType.AlreadyVerified;
        }

        var registrationToken = Encoding.UTF8.GetString(
            WebEncoders.Base64UrlDecode(encodedToken));

        var confirmResult = await _userManager.ConfirmEmailAsync(
            user, registrationToken);
        if (!confirmResult.Succeeded)
        {
            if (confirmResult.Errors.Any(e => e.Code == "InvalidToken"))
            {
                return VerificationResultType.InvalidToken;
            }
            else
            {
                return VerificationResultType.OtherError;
            }
        }

        return VerificationResultType.Success;
    }

    private async Task<string> GetRegistrationHTMLBodyAsync(
        string userId, string token)
    {
        string htmlBody = await _emailService.GetEmailBodyFromTemplateAsync(
            VerifyEmailConstants.Template);
        htmlBody = htmlBody
            .Replace(VerifyEmailConstants.UrlToken, _frontEndOptions.BaseUrl)
            .Replace(
                VerifyEmailConstants.PathToken,
                _frontEndOptions.VerifyRegistrationPath
            )
            .Replace(VerifyEmailConstants.UserToken, userId)
            .Replace(VerifyEmailConstants.TokenToken, token);
        return htmlBody;
    }
}
