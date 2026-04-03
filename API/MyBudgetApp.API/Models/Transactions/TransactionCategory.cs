using MyBudgetApp.API.Models.Users;

namespace MyBudgetApp.API.Models.Transactions;

public class TransactionCategory : BaseModel
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public required string Name { get; set; }
}
