using Microsoft.EntityFrameworkCore;
using MyBudgetApp.API.Models.Transactions;
using MyBudgetApp.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using MyBudgetApp.API.Models.Access;

namespace MyBudgetApp.API.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<User>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public DbSet<SingleTransaction> SingleTransactions { get; set; }
    public DbSet<RecurringTransaction> RecurringTransactions { get; set; }

    public DbSet<SingleTransactionType> SingleTransactionTypes { get; set; }
    public DbSet<TransactionCategory> TransactionCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<TransactionCategory>()
            .HasIndex(c => new { c.UserId, c.Name })
            .IsUnique()
            .HasFilter("deleted_date_utc is null");

        builder.Entity<SingleTransactionType>()
            .HasIndex(t => new { t.UserId, t.Name })
            .IsUnique()
            .HasFilter("deleted_date_utc is null");
    }
}
