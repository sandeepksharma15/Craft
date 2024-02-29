using Craft.MultiTenant.Contexts;

namespace Craft.MultiTenant.Events;

public class TenantEvents
{
    public Func<NotResolvedContext, Task> OnTenantNotResolved { get; set; } = _ => Task.CompletedTask;
    public Func<ResolvedContext, Task> OnTenantResolved { get; set; } = _ => Task.CompletedTask;
}
