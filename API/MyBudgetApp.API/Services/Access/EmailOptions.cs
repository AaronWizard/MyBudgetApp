namespace MyBudgetApp.API.Services.Access;

public class EmailOptions
{
    public const string EmailOptionsKey = "Email";

    public required string FromName { get; set; }
    public required string FromEmail { get; set; }

    public required string Host { get; set; }
    public int Port { get; set; }
    public bool UseSSL { get; set; }

    public required string Username { get; set; }
    public required string Password { get; set; }
}
