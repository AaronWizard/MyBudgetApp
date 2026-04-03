namespace MyBudgetApp.API.Models.Users;

public class RefreshToken : BaseModel
{
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    public required string RefreshTokenHash { get; set; }
}
