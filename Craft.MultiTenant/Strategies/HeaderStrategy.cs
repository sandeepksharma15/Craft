// Set A Request Header with Key as "__TENANT__" and value of the Tenant
using Craft.MultiTenant.Contracts;
using Microsoft.AspNetCore.Http;

namespace Craft.MultiTenant.Strategies;

public class HeaderStrategy(string headerKey = TenantConstants.TenantToken) : ITenantStrategy
{
    private readonly string _headerKey = headerKey;

    public Task<string> GetIdentifierAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        return Task.FromResult(context.Request.Headers[_headerKey].FirstOrDefault());
    }
}
