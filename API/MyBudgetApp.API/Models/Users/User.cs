using MyBudgetApp.API.Models.Transactions;

namespace MyBudgetApp.API.Models.Users;

public class User : BaseModel
{
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public bool RegistrationConfirmed { get; set; }

    #region Transactions

    public ICollection<TransactionCategory> TransactionCategories { get; }
        = new List<TransactionCategory>();
    public ICollection<SingleTransactionType> SingleTransactionTypes { get; }
        = new List<SingleTransactionType>();
    public ICollection<SingleTransaction> SingleTransactions { get; }
        = new List<SingleTransaction>();
    public ICollection<RecurringTransaction> RecurringTransactions { get; }
        = new List<RecurringTransaction>();

    #endregion Transactions

    #region Tokens

    public ICollection<VerifyRegistrationToken> VerifyRegistrationTokens
    { get; } = new List<VerifyRegistrationToken>();
    public ICollection<VerifyLoginCodes> VerifyLoginCodes { get; }
        = new List<VerifyLoginCodes>();
    public ICollection<PasswordResetToken> PasswordResetTokens { get; }
        = new List<PasswordResetToken>();
    public ICollection<RefreshToken> RefreshTokens { get; }
        = new List<RefreshToken>();

    #endregion Tokens
}
