using Microsoft.AspNetCore.Identity;
using MyBudgetApp.API.Models.Access;
using MyBudgetApp.API.Models.Transactions;

namespace MyBudgetApp.API.Models;

public class User : IdentityUser
{
    public DateTime CreateDateUTC { get; set; }
    public DateTime? UpdatedDateUTC { get; set; }
    public DateTime? DeletedDateUTC { get; set; }

    public ICollection<RefreshToken> RefreshTokens { get; }
        = new List<RefreshToken>();

    public ICollection<TransactionCategory> TransactionCategories { get; }
        = new List<TransactionCategory>();
    public ICollection<SingleTransactionType> SingleTransactionTypes { get; }
        = new List<SingleTransactionType>();
    public ICollection<SingleTransaction> SingleTransactions { get; }
        = new List<SingleTransaction>();
    public ICollection<RecurringTransaction> RecurringTransactions { get; }
        = new List<RecurringTransaction>();
}
