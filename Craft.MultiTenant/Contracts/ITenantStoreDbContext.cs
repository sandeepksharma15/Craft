using Microsoft.EntityFrameworkCore;

namespace Craft.MultiTenant.Contracts;

public interface ITenantStoreDbContext<T> where T : class, ITenant, new()
{
    DbSet<T> Tenants { get; set; }
}
