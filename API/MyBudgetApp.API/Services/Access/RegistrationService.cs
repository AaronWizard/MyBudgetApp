using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MyBudgetApp.API.Models;
using MyBudgetApp.API.Options;

namespace MyBudgetApp.API.Services.Access;

public class RegistrationService
{
    public class RegistrationResult
    {
        public bool UserAlreadyExists { get; set; } = false;
        public bool InvalidEmail { get; set; } = false;
        public IEnumerable<string> PasswordErrors { get; set; }
            = Array.Empty<string>();
        public bool OtherError { get; set; } = false;

        public bool InvalidPassword
        {
            get
            {
                return PasswordErrors.Any();
            }
        }

        public bool IsSuccess
        {
            get
            {
                return !UserAlreadyExists
                    && !InvalidEmail
                    && !InvalidPassword
                    && !OtherError;
            }
        }
    }

    public enum VerificationResult
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
            return new RegistrationResult
            {
                UserAlreadyExists = true
            };
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
            return new RegistrationResult
            {
                InvalidEmail = createResult.Errors.Any(
                    e => e.Code == "InvalidEmail"
                ),
                PasswordErrors = createResult.Errors
                    .Where(e => e.Code.StartsWith("Password"))
                    .Select(e => e.Description),

                OtherError = createResult.Errors.Any(e =>
                    (e.Code != "InvalidEmail")
                    && !e.Code.StartsWith("Password")
                )
            };
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

        return new RegistrationResult();
    }

    public async Task<VerificationResult> VerifyRegistrationAsync(
        string userId, string encodedToken)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return VerificationResult.UserNotFound;
        }
        if (await _userManager.IsEmailConfirmedAsync(user))
        {
            return VerificationResult.AlreadyVerified;
        }

        var registrationToken = Encoding.UTF8.GetString(
            WebEncoders.Base64UrlDecode(encodedToken));

        var confirmResult = await _userManager.ConfirmEmailAsync(
            user, registrationToken);
        if (!confirmResult.Succeeded)
        {
            if (confirmResult.Errors.Any(e => e.Code == "InvalidToken"))
            {
                return VerificationResult.InvalidToken;
            }
            else
            {
                return VerificationResult.OtherError;
            }
        }

        return VerificationResult.Success;
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
