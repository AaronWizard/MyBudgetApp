using System;

namespace MyBudgetApp.API.Models.Users;

public class PasswordResetToken : BaseModel
{
    public int UserId { get; set; }
    public required string ResetTokenHash { get; set; }
}
