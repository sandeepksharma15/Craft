using System.Collections.Concurrent;
using Craft.Domain.Contracts;
using Craft.MultiTenant.Contracts;
using Craft.MultiTenant.Enums;
using Craft.MultiTenant.Exceptions;
using Craft.MultiTenant.Models;
using Craft.MultiTenant.Options;
using Microsoft.Extensions.Options;

namespace Craft.MultiTenant.Stores;

public class InMemoryStore<T> : ITenantStore<T> where T : class, ITenant, IEntity, new()
{
    protected readonly InMemoryStoreOptions<T> _options;
    protected readonly ConcurrentDictionary<string, T> _tenantMap;
    protected readonly int tenantId = 1;

    public InMemoryStore(IOptions<InMemoryStoreOptions<T>> options)
    {
        _options = options.Value;

        var stringComparer = StringComparer.OrdinalIgnoreCase;

        if (_options.IsCaseSensitive)
            stringComparer = StringComparer.Ordinal;

        _tenantMap = new ConcurrentDictionary<string, T>(stringComparer);

        foreach (var tenant in _options.Tenants)
        {
            // Configuration is not able to read the TenantId from the configuration file
            // since I have started using a custom data type for TenantId. So, I am using this hack as
            // of now to assign a TenantId to the tenant.
            tenant.Id = tenantId++;

            if (tenant.Id == default)
                throw new MultiTenantException("Missing Tenant Id in options");
            if (_tenantMap.Values.SingleOrDefault(ti => ti.Id == tenant.Id) is not null)
                throw new MultiTenantException("Duplicate Tenant Id in options");
            if (tenant.Identifier.IsNullOrWhiteSpace())
                throw new MultiTenantException("Missing Tenant Identifier in options");
            if (_tenantMap.ContainsKey(tenant.Identifier))
                throw new MultiTenantException("Duplicate Tenant Identifier in options");

            _tenantMap.TryAdd(tenant.Identifier, tenant);
        }
    }

    public async Task<T> AddAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        if (entity.Id == default)
            entity.Id = _tenantMap?.IsEmpty != false ? 1 : _tenantMap.Values.Max(ti => ti.Id) + 1;

        _ = entity.Identifier != null && _tenantMap.TryAdd(entity.Identifier, entity);

        return await Task.FromResult(entity);
    }

    public Task AddRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        if (!_tenantMap.TryRemove(entity.Identifier, out var _))
            throw new MultiTenantException($"Problem deleting the Tenant: {entity.Identifier}");

        await Task.CompletedTask;
    }

    public Task DeleteRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<T>> GetAllAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_tenantMap.Select(x => x.Value).ToList());
    }

    public async Task<T> GetAsync(KeyType id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var result = _tenantMap.Values.SingleOrDefault(ti => ti.Id == id);
        return await Task.FromResult(result);
    }

    public async Task<T> GetByIdentifierAsync(string identifier, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        _tenantMap.TryGetValue(identifier, out var result);
        return await Task.FromResult(result);
    }

    public async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await Task.FromResult(_tenantMap.Count);
    }

    public async Task<T> GetHostAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var result = _tenantMap.Values.SingleOrDefault(ti => ti.Type == TenantType.Host);
        return await Task.FromResult(result);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<T> UpdateAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        var existingTenantInfo = entity.Id != default
            ? await GetAsync(entity.Id, false, cancellationToken)
            : null;

        if (!_tenantMap.TryUpdate(existingTenantInfo.Identifier, entity, existingTenantInfo))
            throw new MultiTenantException($"Problem updating the Tenant: {entity.Identifier}");

        return await Task.FromResult(entity);
    }

    public Task UpdateRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}

public class InMemoryStore(IOptions<InMemoryStoreOptions<Tenant>> options)
    : InMemoryStore<Tenant>(options) { }
