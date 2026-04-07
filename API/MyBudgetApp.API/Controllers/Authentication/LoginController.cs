using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBudgetApp.API.Models;

namespace MyBudgetApp.API.Controllers.Authentication
{
    [Route("api")]
    [ApiController]
    public class LoginController(
        UserManager<User> userManager, SignInManager<User> signInManager
    ) : ControllerBase
    {
        public record LoginRequest(string Email, string Password);

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(
            [FromBody] LoginRequest request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {

            }

            var result = await signInManager.CheckPasswordSignInAsync(
                user, request.Password, false);
            if (!result.Succeeded)
            {

            }

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
