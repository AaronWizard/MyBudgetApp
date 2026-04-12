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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        SetSnakeCasing(builder);

        builder.Entity<TransactionCategory>()
            .HasIndex(c => new { c.UserId, c.Name })
            .IsUnique()
            .HasFilter("deleted_date_utc is null");

        builder.Entity<SingleTransactionType>()
            .HasIndex(t => new { t.UserId, t.Name })
            .IsUnique()
            .HasFilter("deleted_date_utc is null");
    }

    // Use snake casing for the sake of PostgreSQL.
    // Doing it manually instead of using the EFCore.NamingConventions package
    // because that package doesn't work with the tables from Identity
    // framework.
    private void SetSnakeCasing(ModelBuilder builder)
    {
        foreach (var entity in builder.Model.GetEntityTypes())
        {
            entity.SetTableName(ToSnakeCase(entity.GetTableName()));

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(ToSnakeCase(property.GetColumnName()));
            }
            foreach (var key in entity.GetKeys())
            {
                key.SetName(ToSnakeCase(key.GetName()));
            }
            foreach (var fk in entity.GetForeignKeys())
            {
                fk.SetConstraintName(ToSnakeCase(fk.GetConstraintName()));
            }
            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(ToSnakeCase(index.GetDatabaseName()));
            }
        }
    }

    private static string? ToSnakeCase(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        var result = new List<char>();
        var chars = input.Replace(" ", "").ToCharArray();

        foreach (var c in chars)
        {
            if (char.IsUpper(c) && (result.Count > 0))
            {
                result.Add('_');
            }
            result.Add(char.ToLowerInvariant(c));
        }

        return string.Concat(result);
    }
}
