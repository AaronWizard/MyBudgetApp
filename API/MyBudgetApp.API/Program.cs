using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyBudgetApp.API.Data;
using MyBudgetApp.API.Services.Access;
using Scalar.AspNetCore;

const string ConnectionStringKey = "MyBudgetApp";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

#region JWT Services

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

#endregion JWT Services

#region Database Services

var connectionString = builder.Configuration
    .GetConnectionString(ConnectionStringKey)
    ?? throw new InvalidOperationException(
        $"Missing connection string: '{ConnectionStringKey}'"
    );
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(connectionString)
);

#endregion Database Services

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
