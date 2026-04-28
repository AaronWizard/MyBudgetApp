using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using MyBudgetApp.API.Services.Access;

namespace MyBudgetApp.API.Controllers.Authentication
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/user/password")]
    public class PasswordController(PasswordService passwordService)
        : ControllerBase
    {
        [HttpGet("requirements")]
        public PasswordRequirementsOptions PasswordRequirements()
        {
            return passwordService.GetPasswordRequirements();
        }

        [HttpPost("change")]
        public IActionResult ChangePassword()
        {
            return NotFound();
        }

        [HttpPost("forgot")]
        public IActionResult ForgotPassword()
        {
            return NotFound();
        }

        [HttpPost("reset")]
        public IActionResult ResetPassword()
        {
            return NotFound();
        }
    }
}
