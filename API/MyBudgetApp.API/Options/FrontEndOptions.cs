using System;

namespace MyBudgetApp.API.Options;

public class FrontEndOptions
{
    public const string Key = "FrontEnd";

    /// <summary>
    /// Assumed to not have a trailing '/'.
    /// </summary>
    public required string Url { get; set; }
}
