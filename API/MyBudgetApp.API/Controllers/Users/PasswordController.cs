using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetApp.API.Controllers.User
{
    [Route("api/password")]
    [ApiController]
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
