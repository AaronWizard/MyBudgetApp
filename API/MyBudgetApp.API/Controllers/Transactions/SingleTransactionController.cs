using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetApp.API.Controllers.Transactions
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/transaction/single")]
    public class SingleTransactionController : ControllerBase
    {
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
