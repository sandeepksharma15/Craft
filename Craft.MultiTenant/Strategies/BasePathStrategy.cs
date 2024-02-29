using Craft.MultiTenant.Contracts;
using Microsoft.AspNetCore.Http;

namespace Craft.MultiTenant.Strategies;

public class BasePathStrategy : ITenantStrategy
{
    public Task<string> GetIdentifierAsync(HttpContext context)
    {
        ArgumentNullException.ThrowIfNull(context, nameof(context));

        var path = context.Request.Path;

        var pathSegments = path
            .Value?
            .Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

        if (pathSegments is null || pathSegments.Length == 0)
            return Task.FromResult<string>(null);

        return Task.FromResult(pathSegments[0]);
    }
}
