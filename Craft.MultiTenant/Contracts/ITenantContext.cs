using Craft.Domain.Contracts;
using Craft.MultiTenant.Stores;
using Craft.MultiTenant.Strategies;

namespace Craft.MultiTenant.Contracts;

public interface ITenantContext
{
    bool HasResolvedTenant => Tenant != null;
    TenantStrategy Strategy { get; }
    ITenant Tenant { get; }
}

public interface ITenantContext<T> where T : class, ITenant, IEntity, new()
{
    bool HasResolvedTenant => Tenant != null;
    TenantStore<T> Store { get; set; }
    TenantStrategy Strategy { get; set; }
    T Tenant { get; set; }
}
