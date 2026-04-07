using MyBudgetApp.API.Models;

namespace MyBudgetApp.API.Models.Transactions;

public class SingleTransactionType : BaseModel
{
    public string? UserId { get; set; }
    public User? User { get; set; }

    public required string Name { get; set; }
}
