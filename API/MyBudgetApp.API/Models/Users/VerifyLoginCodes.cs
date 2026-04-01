using System;

namespace MyBudgetApp.API.Models.Users;

public class VerifyLoginCodes : BaseModel
{
    public int UserId { get; set; }
    public required string LoginCodeHash { get; set; }
    public required string LoginTokenHash { get; set; }
    public int FailedAttemptsCount { get; set; }
}
