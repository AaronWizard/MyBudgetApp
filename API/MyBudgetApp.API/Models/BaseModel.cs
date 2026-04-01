using System;

namespace MyBudgetApp.API.Models;

public class BaseModel
{
    public int Id { get; set; }
    public DateTime CreateDateUTC { get; set; }
    public DateTime? UpdatedDateUTC { get; set; }
    public DateTime? DeletedDateUTC { get; set; }
}
