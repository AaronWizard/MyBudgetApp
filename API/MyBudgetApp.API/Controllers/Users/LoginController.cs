using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetApp.API.Controllers.User
{
    [Route("api")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost("login")]
        public IActionResult Login()
        {
            return NotFound();
        }

        [HttpPost("login/verify")]
        public IActionResult VerifyLogin()
        {
            return NotFound();
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return NotFound();
        }
    }
}
