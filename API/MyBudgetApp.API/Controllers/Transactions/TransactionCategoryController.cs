using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetApp.API.Controllers.Transactions
{
    [Route("api/transaction/category")]
    [ApiController]
    public class TransactionCategoryController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCategories()
        {
            return NotFound();
        }

        [HttpPost]
        public IActionResult AddCategory()
        {
            return NotFound();
        }

        [HttpPut]
        public IActionResult UpdateCategory()
        {
            return NotFound();
        }

        [HttpDelete]
        public IActionResult DeleteCategory()
        {
            return NotFound();
        }
    }
}
