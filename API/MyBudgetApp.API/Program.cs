using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyBudgetApp.API.Data;
using MyBudgetApp.API.Models;
using MyBudgetApp.API.Models.Transactions;
using MyBudgetApp.API.Services.Access;
using Scalar.AspNetCore;

const string ConnectionStringKey = "MyBudgetApp";
const string SystemTransactionTypesKey = "SystemTransactionTypes";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var connectionString = builder.Configuration
    .GetConnectionString(ConnectionStringKey)
    ?? throw new InvalidOperationException(
        $"Missing connection string: '{ConnectionStringKey}'"
    );

var systemTransactionTypes = builder.Configuration
    .GetSection(SystemTransactionTypesKey).Get<string[]>()
    ?? throw new InvalidOperationException(
        $"Missing connection string: '{ConnectionStringKey}'"
    );

builder.Services.AddDbContext<AppDbContext>(
    options => options
        .UseNpgsql(
            connectionString,
            options =>
                options.MapEnum<RecurringTransaction.PeriodType>("period_type")
        )
        .UseSeeding((context, _) =>
        {
            foreach (var typeName in systemTransactionTypes)
            {
                var typeExists = context.Set<SingleTransactionType>().Any(
                    t => (t.Name == typeName) && string.IsNullOrEmpty(t.UserId)
                );
                if (!typeExists)
                {
                    context.Set<SingleTransactionType>().Add(
                        new SingleTransactionType
                        {
                            UserId = null,
                            Name = typeName,
                            CreateDateUTC = new DateTime(
                                1900, 1, 1, 0, 0, 0, DateTimeKind.Utc
                            )
                        }
                    );
                    context.SaveChanges();
                }
            }
        })
);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
});

var jwtOptionsSection = builder.Configuration.GetSection(
    JwtAccessOptions.JwtOptionsKey);

var jwtOptions = jwtOptionsSection.Get<JwtAccessOptions>()
    ?? throw new InvalidOperationException(
        $"Missing {JwtAccessOptions.JwtOptionsKey} configuration section"
    );

builder.Services.Configure<JwtAccessOptions>(jwtOptionsSection);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.SigningKey)
            )
        };
    });

builder.Services.AddScoped<AccessTokenService>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
else
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
