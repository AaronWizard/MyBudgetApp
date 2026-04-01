using System;

namespace MyBudgetApp.API.Models.Transactions;

public class SingleTransaction : BaseModel
{
    public int UserId { get; set; }
    public int TypeId { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDateUTC { get; set; }
    public int? CategoryId { get; set; }
}
