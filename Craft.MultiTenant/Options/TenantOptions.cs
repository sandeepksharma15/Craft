using Craft.MultiTenant.Events;

namespace Craft.MultiTenant.Options;

public class TenantOptions
{
    public IList<string> IgnoredIdentifiers = [];

    public TenantEvents Events { get; set; } = new TenantEvents();
}
