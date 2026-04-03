using Microsoft.EntityFrameworkCore;
using MyBudgetApp.API.Models.Users;

namespace MyBudgetApp.API.Models.Transactions;

public class RecurringTransaction : BaseModel
{
    public enum PeriodType
    {
        Biweekly = 0,
        Monthly = 1,
        Yearly = 2
    }

    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public required string Name { get; set; }

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    public PeriodType PeriodId { get; set; }

    public int TimesPerPeriod { get; set; }

    public int? CategoryId { get; set; }
    public TransactionCategory? TransactionCategory { get; set; }

    public DateTime StartDateUTC { get; set; }

    public DateTime? EndDateUTC { get; set; }
}
