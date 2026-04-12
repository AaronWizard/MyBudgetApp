using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBudgetApp.API.Models;

namespace MyBudgetApp.API.Controllers.Authentication
{
    [Route("api/register")]
    [ApiController]
    public class RegisterController(UserManager<User> userManager)
        : ControllerBase
    {
        public record RegisterRequest(string Email, string Password);
        public record VerifyRequest(string Email, string Token);

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] RegisterRequest request)
        {
            var existingUser = await userManager.FindByEmailAsync(
                request.Email);
            if (existingUser != null)
            {
                // Vague response for security.
                return Accepted();
            }

            var user = new User
            {
                Email = request.Email,
                CreateDateUTC = DateTime.UtcNow
            };

            var createResult = await userManager.CreateAsync(
                user, request.Password);
            if (!createResult.Succeeded)
            {
                // Vague response for security.
                return Accepted();
            }

            var emailToken
                = await userManager.GenerateEmailConfirmationTokenAsync(user);

            // Vague response for security.
            return Accepted();
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyRegistrationAsync(
            [FromBody] VerifyRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return Unauthorized();
            }
            if (await userManager.IsEmailConfirmedAsync(user))
            {
                return Unauthorized();
            }
            var confirmResult = await userManager.ConfirmEmailAsync(
                user, request.Token);
            if (!confirmResult.Succeeded)
            {
                return Unauthorized();
            }

            return Ok();
        }

        [HttpPost("verify/resend")]
        public IActionResult ResendVerification()
        {
            return NotFound();
        }
    }
}
