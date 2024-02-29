using Craft.MultiTenant.Contracts;

namespace Craft.MultiTenant.Contexts;

public class ResolvedContext
{
    public object Context { get; set; }

    public Type StoreType { get; set; }

    public Type StrategyType { get; set; }

    public ITenant Tenant { get; set; }

    public ResolvedContext() { }

    public ResolvedContext(object context, ITenant tenant, Type strategyType, Type storeType)
    {
        Context = context;
        Tenant = tenant;
        StrategyType = strategyType;
        StoreType = storeType;
    }
}
