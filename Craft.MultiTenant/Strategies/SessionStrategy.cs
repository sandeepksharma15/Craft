// Ensure That app.UseSession(); Is Before app.UseMultiTenancy(); In The Request Pipeline

using Craft.MultiTenant.Contracts;
using Microsoft.AspNetCore.Http;

namespace Craft.MultiTenant.Strategies;

public class SessionStrategy(string tenantKey = TenantConstants.TenantToken) : ITenantStrategy
{
    private readonly string _tenantKey = tenantKey;

    public Task<string> GetIdentifierAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        return Task.FromResult(context.Session.GetString(_tenantKey));
    }
}
