// builder.Services.AddMultiTenancy<Tenant>().WithStaticStrategy("localhost").

using Craft.MultiTenant.Contracts;
using Microsoft.AspNetCore.Http;

namespace Craft.MultiTenant.Strategies;

public class StaticStrategy(string identifier) : ITenantStrategy
{
    private readonly string _identifier = identifier;

    public int Priority { get => -1000; }

    public Task<string> GetIdentifierAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));
        return Task.FromResult(_identifier);
    }
}
