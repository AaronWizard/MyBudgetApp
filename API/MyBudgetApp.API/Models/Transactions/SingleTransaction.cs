using Microsoft.EntityFrameworkCore;
using MyBudgetApp.API.Models;

namespace MyBudgetApp.API.Models.Transactions;

public class SingleTransaction : BaseModel
{
    public required string UserId { get; set; }
    public User User { get; set; } = null!;

    public int TypeId { get; set; }
    public SingleTransactionType SingleTransactionType { get; set; } = null!;

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public DateTime TransactionDateUTC { get; set; }

    public int? TransactionCategoryId { get; set; }
    public TransactionCategory? TransactionCategory { get; set; }
}
