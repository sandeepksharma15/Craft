// builder.Services.AddMultiTenancy<Tenant>().WithDelegateStrategy(_ => Task.FromResult("localhost"));

using Craft.MultiTenant.Contracts;
using Microsoft.AspNetCore.Http;

namespace Craft.MultiTenant.Strategies;

public class DelegateStrategy(Func<object, Task<string>> doStrategy) : ITenantStrategy
{
    private readonly Func<object, Task<string>> _doStrategy = doStrategy;

    public Task<string> GetIdentifierAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        return _doStrategy(context);
    }
}
