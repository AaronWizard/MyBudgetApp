using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetApp.API.Controllers.Authentication
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/user/password")]
    public class PasswordController : ControllerBase
    {
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
