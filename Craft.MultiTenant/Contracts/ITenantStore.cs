using Craft.Domain.Contracts;
using Craft.Domain.Repositories;
using Craft.MultiTenant.Models;

namespace Craft.MultiTenant.Contracts;

public interface ITenantStore<T> : IChangeRepository<T> where T : class, ITenant, IEntity, new()
{
    Task<T> GetByIdentifierAsync(string identifier, bool includeDetails = false, CancellationToken cancellationToken = default);

    Task<T> GetHostAsync(bool includeDetails = false, CancellationToken cancellationToken = default);
}

public interface ITenantStore : ITenantStore<Tenant>;
