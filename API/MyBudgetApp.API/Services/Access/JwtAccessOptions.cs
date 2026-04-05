namespace MyBudgetApp.API.Services.Access;

public class JwtAccessOptions
{
    public const string JwtOptionsKey = "Jwt";

    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string SigningKey { get; set; }
    public required string SecretKey { get; set; }
    public required int AccessMinutes { get; set; }
}
