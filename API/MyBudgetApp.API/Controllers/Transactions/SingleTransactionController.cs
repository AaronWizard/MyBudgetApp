using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetApp.API.Controllers.Transactions
{
    [Route("api/transaction/single")]
    [ApiController]
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
