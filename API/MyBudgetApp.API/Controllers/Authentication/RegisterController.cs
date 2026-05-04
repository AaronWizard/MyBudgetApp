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

            var invalidEmail
                = (verificationResult.Type
                    == RegistrationService.RegistrationResultType.InvalidEmail)
                || (verificationResult.Type
                    == RegistrationService.RegistrationResultType
                        .InvalidEmailAndPassword);

            // Only report invalid emails and passwords for security.
            if (invalidEmail || verificationResult.PasswordErrors.Any())
            {
                return BadRequest(
                    new RegistrationErrorResponse(
                        invalidEmail,
                        verificationResult.PasswordErrors
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

            IActionResult result;
            switch (verificationResult)
            {
                case RegistrationService.VerificationResultType.Success:
                    result = Ok();
                    break;
                case RegistrationService.VerificationResultType.UserNotFound:
                    result = NotFound();
                    break;
                case RegistrationService.VerificationResultType.AlreadyVerified:
                    result = Conflict();
                    break;
                case RegistrationService.VerificationResultType.InvalidToken:
                    result = BadRequest();
                    break;
                default:
                    result = StatusCode(
                        StatusCodes.Status500InternalServerError);
                    break;
            }
            return result;
        }

        [HttpPost("verify/resend")]
        public IActionResult ResendVerification()
        {
            return NotFound();
        }
    }
}
