using Craft.MultiTenant.Contracts;
using Microsoft.AspNetCore.Http;

namespace Craft.MultiTenant.Strategies;

public class SubDomainStrategy : ITenantStrategy
{
    public Task<string> GetIdentifierAsync(HttpContext context)
    {
        string _identifier = string.Empty;

        var _hostList = context.Request.Host.Host.Split('.');

        if (_hostList.Length > 2)
            _identifier = _hostList[0].ToLower();

        return Task.FromResult(_identifier);
    }
}
