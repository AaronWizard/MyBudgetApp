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
        public record RegistrationRequest(string Email, string Password);
        public record RegistrationErrorResponse(
            bool InvalidEmail, string[] PasswordErrors
        );

        public record VerifyRequest(string UserId, string Token);

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(
            [FromBody] RegistrationRequest request)
        {
            var verificationResult = await registrationService
                .RegisterUserAsync(request.Email, request.Password);

            // Only report invalid emails and passwords for security.
            if (verificationResult.InvalidEmail
                || verificationResult.InvalidPassword)
            {
                return BadRequest(
                    new RegistrationErrorResponse(
                        verificationResult.InvalidEmail,
                        verificationResult.PasswordErrors.ToArray()
                    )
                );
            }
            else
            {
                return Accepted();
            }
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyRegistrationAsync(
            [FromBody] VerifyRequest request)
        {
            var verificationResult = await registrationService
                .VerifyRegistrationAsync(request.UserId, request.Token);

            if (verificationResult
                == RegistrationService.ConfirmationResult.Success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("verify/resend")]
        public IActionResult ResendVerification()
        {
            return NotFound();
        }
    }
}
