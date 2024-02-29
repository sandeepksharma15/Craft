using Craft.Domain.Contracts;
using Craft.MultiTenant.Contracts;

namespace Craft.MultiTenant.Stores;

public class TenantStore<T> where T : class, ITenant, IEntity, new()
{
    public ITenantStore<T> Store { get; internal set; }

    public Type StoreType { get; internal set; }

    public TenantStore() { }

    public TenantStore(ITenantStore<T> store, Type storeType)
    {
        Store = store;
        StoreType = storeType;
    }
}
