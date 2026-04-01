using System;

namespace MyBudgetApp.API.Models.Transactions;

public class TransactionCategory : BaseModel
{
    public int UserId { get; set; }
    public required string Name { get; set; }
}
