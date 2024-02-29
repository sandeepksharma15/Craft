using Craft.Domain.Contracts;
using Craft.MultiTenant.Contracts;
using Craft.MultiTenant.Enums;
using Craft.MultiTenant.Exceptions;
using Microsoft.Extensions.Logging;

namespace Craft.MultiTenant.Stores;

public class StoreWrapper<T> : ITenantStore<T> where T : class, ITenant, IEntity, new()
{
    private readonly ILogger _logger;
    private readonly ITenantStore<T> _store;

    public StoreWrapper(ITenantStore<T> store, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(store, nameof(store));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        _logger = logger;
        _store = store;
    }

    public async Task<T> AddAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        ArgumentNullException.ThrowIfNull(entity.Identifier, nameof(entity.Identifier));

        var existing = await GetAsync(entity.Id, false, cancellationToken);

        if (existing != null || await GetByIdentifierAsync(entity.Identifier, false, cancellationToken) != null)
            _logger.LogDebug("AddAsync: Tenant already exists. Id: \"{Id}\", Identifier: \"{Identifier}\"", entity.Id, entity.Identifier);

        // Check that there is only one host tenant
        var host = await GetHostAsync(false, cancellationToken);

        if (host is not null && entity.Type == TenantType.Host)
            _logger.LogDebug("AddAsync: There is already a host tenant. Id: \"{Id}\", Identifier: \"{Identifier}\"", entity.Id, entity.Identifier);

        return await _store.AddAsync(entity, autoSave, cancellationToken);
    }

    public Task AddRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        ArgumentNullException.ThrowIfNull(entity.Identifier, nameof(entity.Identifier));

        try
        {
            await _store.DeleteAsync(entity, autoSave, cancellationToken);

            _logger.LogDebug("DeleteAsync: Tenant Identifier: \"{Identifier}\" removed", entity.Identifier);
        }
        catch (Exception e)
        {
            _logger.LogDebug("DeleteAsync: Unable to remove Tenant Identifier: \"{Identifier}\"", entity.Identifier);
            _logger.LogError(e, "Exception in DeleteAsync");
        }
    }

    public Task DeleteRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<T>> GetAllAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        var result = new List<T>();

        try
        {
            result = await _store.GetAllAsync(includeDetails, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception in GetAllAsync");
        }

        return result;
    }

    public async Task<T> GetAsync(KeyType id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        T result = null;

        try
        {
            result = await _store.GetAsync(id, includeDetails, cancellationToken);

            if (result == null)
                _logger.LogDebug("GetAsync: Unable to find Tenant Id \"{TenantId}\".", id);
            else
                _logger.LogDebug("GetAsync: Tenant Id \"{TenantId}\" found.", id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception in GetAsync");
        }

        return result;
    }

    public async Task<T> GetByIdentifierAsync(string identifier, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        T result = null;

        ArgumentNullException.ThrowIfNull(identifier, nameof(identifier));

        try
        {
            result = await _store.GetByIdentifierAsync(identifier, includeDetails, cancellationToken);

            if (result != null)
                _logger.LogDebug("GetByIdentifierAsync: Tenant found with identifier \"{TenantIdentifier}\"", identifier);
            else
                _logger.LogDebug("GetByIdentifierAsync: Unable to find Tenant with identifier \"{TenantIdentifier}\"", identifier);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Exception in TryGetByIdentifierAsync");
        }

        return result;
    }

    public Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return _store.GetCountAsync(cancellationToken);
    }

    public Task<T> GetHostAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<T> UpdateAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        ArgumentNullException.ThrowIfNull(entity.Identifier, nameof(entity.Identifier));

        try
        {
            // Check that there is no duplicate identifier
            ITenant tenant = await GetByIdentifierAsync(entity.Identifier, false, cancellationToken);
            if (tenant != null && tenant.Id != entity.Id)
                throw new MultiTenantException($"Tenant with identifier \"{entity.Identifier}\" already exists.");

            // Check that there is only one host tenant
            if (entity.Type == TenantType.Host)
            {
                ITenant host = await GetHostAsync(false, cancellationToken);
                if (host != null && host.Id != entity.Id)
                    throw new MultiTenantException("There is already a host tenant.");
            }

            var existing = await GetAsync(entity.Id, false, cancellationToken);

            if (existing == null)
                _logger.LogDebug("UpdateAsync: Tenant Id: \"{TenantId}\" not found", entity.Id);
            else
            {
                entity = await _store.UpdateAsync(entity, autoSave, cancellationToken);

                _logger.LogDebug("UpdateAsync: Tenant Id: \"{TenantId}\" updated", entity.Id);
            }
        }
        catch (Exception e)
        {
            _logger.LogDebug("UpdateAsync: Unable to update Tenant Id: \"{TenantId}\"", entity.Id);
            _logger.LogError(e, "Exception in UpdateAsync");
        }

        return entity;
    }

    public Task UpdateRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
