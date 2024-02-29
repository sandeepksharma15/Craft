using Craft.Domain.Contracts;
using Craft.MultiTenant.Contracts;

namespace Craft.MultiTenant.Services;

public class TenantContextAccessor<T> : ITenantContextAccessor<T>, ITenantContextAccessor
    where T : class, ITenant, IEntity, new()
{
    private static readonly AsyncLocal<ITenantContext<T>> _asyncLocalContext = new();

    public ITenantContext<T> TenantContext
    {
        get => _asyncLocalContext.Value;
        set => _asyncLocalContext.Value = value;
    }

    ITenantContext ITenantContextAccessor.TenantContext
    {
        get => TenantContext as ITenantContext;
        set => TenantContext = value as ITenantContext<T> ?? TenantContext;
    }
}
