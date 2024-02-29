using Craft.MultiTenant.Enums;

namespace Craft.MultiTenant.Options;

public class MultiTenantOptions
{
    public TenantDbType DbType { get; set; } = TenantDbType.PerTenant;
    public bool IsEnabled { get; set; }
}
