using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyBudgetApp.API.Data;
using Scalar.AspNetCore;

#region Helper Functions

string GetConfigurationString(WebApplicationBuilder builder, string key)
{
    var result = builder.Configuration[key];
    return result ?? throw new InvalidOperationException(
        $"Missing configuration key: '{key}'"
    );
}

string GetConnectionString(WebApplicationBuilder builder, string key)
{
    var result = builder.Configuration.GetConnectionString(key);
    return result ?? throw new InvalidOperationException(
        $"Missing connection string: '{key}'"
    );
}

#endregion Helper Functions

const string ValidIssuerKey = "Jwt:Issuer";
const string ValidAudienceKey = "Jwt:Audience";
const string SigningKeyKey = "Jwt:SigningKey";

const string ConnectionStringKey = "MyBudgetApp";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var issuer = GetConfigurationString(builder, ValidIssuerKey);
var audience = GetConfigurationString(builder, ValidAudienceKey);
var signingKey = GetConfigurationString(builder, SigningKeyKey);

var connectionString = GetConnectionString(builder, ConnectionStringKey);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(signingKey)
            )
        };
    });

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseNpgsql(connectionString)
);

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
