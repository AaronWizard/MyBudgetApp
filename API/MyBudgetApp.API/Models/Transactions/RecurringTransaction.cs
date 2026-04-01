using System;

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
    public required string Name { get; set; }
    public decimal Amount { get; set; }
    public PeriodType PeriodId { get; set; }
    public int TimesPerPeriod { get; set; }
    public int? CategoryId { get; set; }
    public DateTime StartDateUTC { get; set; }
    public DateTime? EndDateUTC { get; set; }
}
