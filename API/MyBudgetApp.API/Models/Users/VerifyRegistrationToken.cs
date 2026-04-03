namespace MyBudgetApp.API.Models.Users;

public class VerifyRegistrationToken : BaseModel
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public required string RegistrationTokenHash { get; set; }
}
