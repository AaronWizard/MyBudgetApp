using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetApp.API.Controllers.Transactions
{
    [Route("api/transaction/average")]
    [ApiController]
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
