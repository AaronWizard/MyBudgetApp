namespace MyBudgetApp.API.Options;

public class FrontEndOptions
{
    public const string Key = "FrontEnd";

    /// <summary>
    /// Assumed to not have a trailing '/'.
    /// </summary>
    public required string BaseUrl { get; set; }

    /// <summary>
    /// Assumed to not have a '/' at either the start nor the end.
    /// </summary>
    public required string VerifyRegistrationPath { get; set; }
}
