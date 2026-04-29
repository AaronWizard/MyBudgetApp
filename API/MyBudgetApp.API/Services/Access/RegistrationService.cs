using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using MyBudgetApp.API.Models;
using MyBudgetApp.API.Options;

namespace MyBudgetApp.API.Services.Access;

public class RegistrationService
{
    const string VerifyEmailSubject = "[My Budget App] Verify Your Account";

    private readonly UserManager<User> _userManager;
    private readonly EmailService _emailService;
    private readonly FrontEndOptions _frontEndOptions;

    public RegistrationService(
        UserManager<User> userManager, EmailService emailService,
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

        await _emailService.SendEmailAsync(
            email, VerifyEmailSubject,
            $"Verify your account: {_frontEndOptions.Url}/{encodedToken}"
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
}
