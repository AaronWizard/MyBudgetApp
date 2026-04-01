using System;

namespace MyBudgetApp.API.Models.Users;

public class VerifyRegistrationToken : BaseModel
{
    public int UserId { get; set; }
    public required string RegistrationTokenHash { get; set; }
}
