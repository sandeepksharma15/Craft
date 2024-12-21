using Craft.Domain.Contracts;
using Craft.Domain.Repositories;
using Craft.Infrastructure.CacheService;
using Craft.MultiTenant.Contracts;
using Craft.MultiTenant.Models;
using Microsoft.EntityFrameworkCore;

namespace Craft.MultiTenant.Stores;

public class CacheStore(ICacheService cacheService, IRepository<Tenant> tenantRepository)
    : CacheStore<Tenant>(cacheService, tenantRepository), ITenantStore
{
}

public class CacheStore<T>(ICacheService cacheService, IRepository<T> tenantRepository)
    : ITenantStore<T> where T : class, ITenant, IEntity, new()
{
    private const string _cacheKey = "_TENANT_STORE";

    private readonly ICacheService _cacheService = cacheService;
    private readonly IRepository<T> _tenantRepository = tenantRepository;

    private async Task<List<T>> GetTenantList()
    {
        // Get From The Cache
        (bool hasKey, List<T> tenants) = _cacheService.TryGet<List<T>>(_cacheKey);

        // If Key Is Missing, Get Again From Repository
        if (!hasKey)
            tenants = await _tenantRepository.GetAllAsync();

        // Set In Cache
        _cacheService.Set(_cacheKey, tenants);

        return tenants;
    }

    public Task<T> AddAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task AddRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<T>> GetAllAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await GetTenantList();
    }

    public async Task<T> GetAsync(KeyType id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var tenants = await GetTenantList();

        return tenants.Find(t => t.Id == id);
    }

    public async Task<T> GetByIdentifierAsync(string identifier, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var tenants = await GetTenantList();

        return tenants.Find(t => t.Identifier == identifier);
    }

    public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return (await GetTenantList()).Count;
    }

    public Task<T> GetHostAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<T> UpdateAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<DbContext> GetDbContextAsync()
    {
        throw new NotImplementedException();
    }

    public Task<DbSet<T>> GetDbSetAsync()
    {
        throw new NotImplementedException();
    }

    public int SaveChanges()
    {
        throw new NotImplementedException();
    }
}
