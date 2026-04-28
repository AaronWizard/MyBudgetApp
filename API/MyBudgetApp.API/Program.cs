using System.Text;
using Asp.Versioning;
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

#region Database services

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
        .UseSnakeCaseNamingConvention()
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

#endregion Database services

#region Identity services

var passwordRequirementsSection = builder.Configuration.GetSection(
    PasswordRequirementsOptions.Key);

var passwordRequirements = passwordRequirementsSection
    .Get<PasswordRequirementsOptions>()
    ?? throw new InvalidOperationException(
        $"Missing {PasswordRequirementsOptions.Key} configuration section"
    );

builder.Services.Configure<PasswordRequirementsOptions>(
    passwordRequirementsSection);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddSignInManager();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = passwordRequirements.RequireDigit;
    options.Password.RequireLowercase = passwordRequirements.RequireLowercase;
    options.Password.RequireNonAlphanumeric
        = passwordRequirements.RequireNonAlphanumeric;
    options.Password.RequireUppercase = passwordRequirements.RequireUppercase;
    options.Password.RequiredLength = passwordRequirements.RequiredLength;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    options.SignIn.RequireConfirmedEmail = true;

    options.User.RequireUniqueEmail = true;
});

#endregion Identity services

#region JWT services

var jwtOptionsSection = builder.Configuration.GetSection(
    JwtAccessOptions.Key);

var jwtOptions = jwtOptionsSection.Get<JwtAccessOptions>()
    ?? throw new InvalidOperationException(
        $"Missing {JwtAccessOptions.Key} configuration section"
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

#endregion JWT services

builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection(EmailOptions.EmailOptionsKey));

builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<RegistrationService>();
builder.Services.AddScoped<AccessTokenService>();

builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1.0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
});

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
