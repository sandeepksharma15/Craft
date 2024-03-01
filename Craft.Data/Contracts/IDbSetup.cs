using Craft.MultiTenant.Models;

namespace Craft.Data.Contracts;

public interface IDbSetup
{
    Task SetupAppDbAsync(Tenant tenant, CancellationToken cancellationToken);

    Task SetupTenantDbAsync(CancellationToken cancellationToken);
}
