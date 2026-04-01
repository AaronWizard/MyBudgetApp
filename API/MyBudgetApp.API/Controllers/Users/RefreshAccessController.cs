using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetApp.API.Controllers.User
{
    [Route("api/refresh-access")]
    [ApiController]
    public class RefreshAccessController : ControllerBase
    {
        [HttpPost]
        public IActionResult RefreshAccess()
        {
            return NotFound();
        }
    }
}
