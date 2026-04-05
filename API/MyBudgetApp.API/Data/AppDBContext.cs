using Microsoft.EntityFrameworkCore;
using MyBudgetApp.API.Models.Transactions;
using MyBudgetApp.API.Models.Users;

namespace MyBudgetApp.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    #region Transactions

    public DbSet<SingleTransaction> SingleTransactions { get; set; }
    public DbSet<RecurringTransaction> RecurringTransactions { get; set; }

    public DbSet<SingleTransactionType> SingleTransactionTypes { get; set; }
    public DbSet<TransactionCategory> TransactionCategories { get; set; }

    #endregion Transactions

    #region User Tokens

    public DbSet<VerifyRegistrationToken> VerifyRegistrationTokens { get; set; }
    public DbSet<VerifyLoginCodes> VerifyLoginCodes { get; set; }
    public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    #endregion User Tokens
}
