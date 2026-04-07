using MyBudgetApp.API.Models;

namespace MyBudgetApp.API.Models.Transactions;

public class TransactionCategory : BaseModel
{
    public required string UserId { get; set; }
    public User User { get; set; } = null!;

    public required string Name { get; set; }
}
