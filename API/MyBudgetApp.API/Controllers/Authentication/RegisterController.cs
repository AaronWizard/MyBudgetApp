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

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] RegisterRequest request)
        {
            var existingUser = await userManager.FindByEmailAsync(
                request.Email);
            if (existingUser != null)
            {
                // Don't actually create the user.
                // Return vague response for security.
                return Accepted();
            }

            var user = new User
            {
                Email = request.Email,
                CreateDateUTC = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                // Return vague response for security.
                return Accepted();
            }

            return Accepted();
        }

        [HttpPost("verify")]
        public IActionResult VerifyRegistration()
        {
            return NotFound();
        }

        [HttpPost("verify/resend")]
        public IActionResult ResendVerification()
        {
            return NotFound();
        }
    }
}
