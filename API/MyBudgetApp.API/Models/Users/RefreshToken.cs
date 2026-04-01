using System;

namespace MyBudgetApp.API.Models.Users;

public class RefreshToken : BaseModel
{
    public int UserId { get; set; }
    public required string RefreshTokenHash { get; set; }
}
