namespace MyBudgetApp.API.Options;

public class PasswordRequirementsOptions
{
    public const string Key = "PasswordRequirements";

    public bool RequireDigit { get; set; }
    public bool RequireLowercase { get; set; }
    public bool RequireNonAlphanumeric { get; set; }
    public bool RequireUppercase { get; set; }
    public int RequiredLength { get; set; }
}
