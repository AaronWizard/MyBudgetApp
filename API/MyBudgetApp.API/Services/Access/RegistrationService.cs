using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MyBudgetApp.API.Models;

namespace MyBudgetApp.API.Services.Access;

public class RegistrationService
{
    private readonly PasswordRequirementsOptions _passwordOptions;
    private readonly UserManager<User> _userManager;
    private readonly EmailService _emailService;

    public RegistrationService(IOptions<PasswordRequirementsOptions> options,
        UserManager<User> userManager, EmailService emailService)
    {
        _passwordOptions = options.Value;
        _userManager = userManager;
        _emailService = emailService;
    }

    const string VerifyEmailSubject = "[My Budget App] Verify Your Account";

    public PasswordRequirementsOptions GetPasswordRequirements()
    {
        return _passwordOptions;
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
        var emailToken
            = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        await _emailService.SendEmailAsync(
            email, VerifyEmailSubject,
            $"Verify your account with the token '{emailToken}'"
        );

        return true;
    }

    public async Task<bool> VerifyRegistrationAsync(string email, string token)
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

        var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
        if (!confirmResult.Succeeded)
        {
            return false;
        }

        return true;
    }
}
