using Craft.Domain.Contracts;
using Craft.MultiTenant.Contracts;
using Craft.MultiTenant.Models;
using Craft.MultiTenant.Stores;
using Craft.MultiTenant.Strategies;

namespace Craft.MultiTenant.Contexts;

public class TenantContext<T> : ITenantContext<T>, ITenantContext where T : class, ITenant, IEntity, new()
{
    public TenantStore<T> Store { get; set; }

    public TenantStrategy Strategy { get; set; }

    public T Tenant { get; set; }

    ITenant ITenantContext.Tenant => Tenant;

    public TenantContext()
    { }

    public TenantContext(T tenant, TenantStore<T> store, TenantStrategy strategy)
    {
        Tenant = tenant;
        Store = store;
        Strategy = strategy;
    }
}

public class TenantContext : TenantContext<Tenant>;
