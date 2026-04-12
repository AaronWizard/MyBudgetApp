using Microsoft.EntityFrameworkCore;
using MyBudgetApp.API.Models.Transactions;
using MyBudgetApp.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Text.RegularExpressions;

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
            entity.SetTableName(CamelCaseToSnakeCase(entity.GetTableName()));

            foreach (var property in entity.GetProperties())
            {
                property.SetColumnName(CamelCaseToSnakeCase(property.GetColumnName()));
            }
            foreach (var key in entity.GetKeys())
            {
                key.SetName(CamelCaseToSnakeCase(key.GetName()));
            }
            foreach (var fk in entity.GetForeignKeys())
            {
                fk.SetConstraintName(CamelCaseToSnakeCase(fk.GetConstraintName()));
            }
            foreach (var index in entity.GetIndexes())
            {
                index.SetDatabaseName(CamelCaseToSnakeCase(index.GetDatabaseName()));
            }
        }
    }

    private static string? CamelCaseToSnakeCase(string? input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        string result = Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2");
        result = Regex.Replace(result, @"([A-Z]+)([A-Z][a-z])", "$1_$2");

        return result.ToLower();
    }
}
