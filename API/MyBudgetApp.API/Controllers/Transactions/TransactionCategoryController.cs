using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace MyBudgetApp.API.Controllers.Transactions
{
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/transaction/category")]
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
