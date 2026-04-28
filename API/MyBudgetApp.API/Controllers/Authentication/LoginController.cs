using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBudgetApp.API.Models;
using MyBudgetApp.API.Services.Access;

namespace MyBudgetApp.API.Controllers.Authentication
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/user")]
    public class LoginController(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        AccessTokenService tokenService
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
                return Unauthorized();
            }
            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                return Unauthorized();
            }
            var checkPassword = await signInManager.CheckPasswordSignInAsync(
                user, request.Password, false);
            if (!checkPassword.Succeeded)
            {
                return Unauthorized();
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
