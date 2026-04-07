using Microsoft.EntityFrameworkCore;
using MyBudgetApp.API.Models.Transactions;
using MyBudgetApp.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MyBudgetApp.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User>(options)
{
    public DbSet<SingleTransaction> SingleTransactions { get; set; }
    public DbSet<RecurringTransaction> RecurringTransactions { get; set; }

    public DbSet<SingleTransactionType> SingleTransactionTypes { get; set; }
    public DbSet<TransactionCategory> TransactionCategories { get; set; }
}
