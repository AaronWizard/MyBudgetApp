using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetApp.API.Controllers.Transactions
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/transaction/average")]
    public class TransactionAveragesController : ControllerBase
    {
        [HttpGet("by-period")]
        public IActionResult GetByPeriod()
        {
            return NotFound();
        }

        [HttpGet("by-date-range")]
        public IActionResult GetByDateRange()
        {
            return NotFound();
        }
    }
}
