using Microsoft.Extensions.Options;
using MyBudgetApp.API.Options;

namespace MyBudgetApp.API.Services.Access;

public class PasswordService
{
    private readonly PasswordRequirementsOptions _passwordOptions;

    public PasswordService(IOptions<PasswordRequirementsOptions> options)
        => _passwordOptions = options.Value;

    public PasswordRequirementsOptions GetPasswordRequirements()
    {
        return _passwordOptions;
    }
}
