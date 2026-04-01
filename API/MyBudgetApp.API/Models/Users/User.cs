using System;

namespace MyBudgetApp.API.Models.Users;

public class User : BaseModel
{
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public bool RegistrationConfirmed { get; set; }
}
