using Microsoft.AspNetCore.Identity;
using MyBudgetApp.API.Models;

namespace MyBudgetApp.API.Services.Access;

public class RegistrationService(
    UserManager<User> userManager, EmailService emailService
)
{
    const string VerifyEmailSubject = "[My Budget App] Verify Your Account";

    public async Task<bool> RegisterUserAsync(string email, string password)
    {
        var existingUser = await userManager.FindByEmailAsync(email);
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
        var createResult = await userManager.CreateAsync(user, password);
        if (!createResult.Succeeded)
        {
            return false;
        }

        // Send registration email
        var emailToken
            = await userManager.GenerateEmailConfirmationTokenAsync(user);

        await emailService.SendEmailAsync(
            email, VerifyEmailSubject,
            $"Verify your account with the token '{emailToken}'"
        );

        return true;
    }

    public async Task<bool> VerifyRegistrationAsync(string email, string token)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return false;
        }
        if (await userManager.IsEmailConfirmedAsync(user))
        {
            return false;
        }

        var confirmResult = await userManager.ConfirmEmailAsync(user, token);
        if (!confirmResult.Succeeded)
        {
            return false;
        }

        return true;
    }
}
