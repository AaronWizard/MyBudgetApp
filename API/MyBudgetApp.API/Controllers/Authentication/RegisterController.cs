using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MyBudgetApp.API.Services.Access;

namespace MyBudgetApp.API.Controllers.Authentication
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/user/register")]
    public class RegisterController(RegistrationService registrationService)
        : ControllerBase
    {
        public record RegisterRequest(string Email, string Password);
        public record VerifyRequest(string Email, string Token);

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] RegisterRequest request)
        {
            await registrationService.RegisterUserAsync(
                request.Email, request.Password);
            // Vague response for security.
            return Accepted();
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyRegistrationAsync(
            [FromBody] VerifyRequest request)
        {
            var verificationSucceeded = await registrationService
                .VerifyRegistrationAsync(request.Email, request.Token);
            if (!verificationSucceeded)
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
