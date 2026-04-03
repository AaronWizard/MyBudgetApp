namespace MyBudgetApp.API.Models.Users;

public class VerifyLoginCodes : BaseModel
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public required string LoginCodeHash { get; set; }
    public required string LoginTokenHash { get; set; }
    public int FailedAttemptsCount { get; set; }
}
