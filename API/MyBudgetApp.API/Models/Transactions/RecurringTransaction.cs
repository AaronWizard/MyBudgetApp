using Microsoft.EntityFrameworkCore;
using MyBudgetApp.API.Models;

namespace MyBudgetApp.API.Models.Transactions;

public class RecurringTransaction : BaseModel
{
    public enum PeriodType
    {
        Biweekly = 1,
        Monthly = 2,
        Yearly = 3
    }

    public required string UserId { get; set; }
    public User User { get; set; } = null!;

    public required string Name { get; set; }

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public PeriodType PeriodTypeId { get; set; }

    public int TimesPerPeriod { get; set; }

    public int? TransactionCategoryId { get; set; }
    public TransactionCategory? TransactionCategory { get; set; }

    public DateTime StartDateUTC { get; set; }

    public DateTime? EndDateUTC { get; set; }
}
