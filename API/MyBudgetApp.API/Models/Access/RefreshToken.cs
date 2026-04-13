using System;

namespace MyBudgetApp.API.Models.Access;

public class RefreshToken
{
    public int Id { get; set; }

    public required string UserId { get; set; }
    public User User { get; set; } = null!;

    public required string RefreshTokenHash { get; set; }

    public DateTime CreatedDateUTC { get; set; }
    public DateTime ExpiryDateUTC { get; set; }
    public DateTime? RevocationDateUTC { get; set; }
}
