using Microsoft.EntityFrameworkCore;
using MyBudgetApp.API.Models.Users;

namespace MyBudgetApp.API.Models.Transactions;

public class SingleTransaction : BaseModel
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public int TypeId { get; set; }
    public SingleTransactionType SingleTransactionType { get; set; } = null!;

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public DateTime TransactionDateUTC { get; set; }

    public int? CategoryId { get; set; }
    public TransactionCategory? TransactionCategory { get; set; }
}
