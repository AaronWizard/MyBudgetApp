using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetApp.API.Controllers.User
{
    [Route("api/register")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        [HttpPost]
        public IActionResult Register()
        {
            return NotFound();
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
