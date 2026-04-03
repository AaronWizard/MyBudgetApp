namespace MyBudgetApp.API.Models.Users;

public class PasswordResetToken : BaseModel
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public required string ResetTokenHash { get; set; }
}
