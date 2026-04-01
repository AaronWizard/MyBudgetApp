using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetApp.API.Controllers.Transactions
{
    [Route("api/transaction/single/type")]
    [ApiController]
    public class SingleTransactionTypeController : ControllerBase
    {
        [HttpGet("system")]
        public IActionResult GetSystemTypes()
        {
            return NotFound();
        }

        [HttpGet("user")]
        public IActionResult GetUserTypes()
        {
            return NotFound();
        }

        [HttpPost("user")]
        public IActionResult AddUserType()
        {
            return NotFound();
        }

        [HttpPut("user")]
        public IActionResult UpdateUserType()
        {
            return NotFound();
        }

        [HttpDelete("user")]
        public IActionResult DeleteUserType()
        {
            return NotFound();
        }
    }
}
