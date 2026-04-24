using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetApp.API.Controllers.Authentication
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/refresh-access")]
    public class RefreshAccessController : ControllerBase
    {
        [HttpPost]
        public IActionResult RefreshAccess()
        {
            return NotFound();
        }
    }
}
