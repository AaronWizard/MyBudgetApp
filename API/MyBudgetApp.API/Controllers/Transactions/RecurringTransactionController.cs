using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetApp.API.Controllers.Transactions
{
    [Route("api/transaction/recurring")]
    [ApiController]
    public class RecurringTransactionController : ControllerBase
    {
        [HttpGet("period-type")]
        public IActionResult GetPeriodTypes()
        {
            return NotFound();
        }

        [HttpGet]
        public IActionResult Get()
        {
            return NotFound();
        }

        [HttpPost]
        public IActionResult Add()
        {
            return NotFound();
        }

        [HttpPut]
        public IActionResult Update()
        {
            return NotFound();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return NotFound();
        }
    }
}
