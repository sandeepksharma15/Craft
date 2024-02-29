using Craft.Domain.Contracts;
using Craft.MultiTenant.Contracts;
using Craft.MultiTenant.Enums;
using Craft.MultiTenant.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Craft.MultiTenant.Stores;

public class DbStore<TStoreDbContext, T>(TStoreDbContext dbContext) : ITenantStore<T>
        where TStoreDbContext : DbContext, ITenantStoreDbContext<T>
        where T : class, ITenant, IEntity, new()
{
    internal readonly TStoreDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public virtual async Task<T> AddAsync(T tenant, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        var result = await _dbContext
            .Tenants
            .AddAsync(tenant, cancellationToken)
            .ConfigureAwait(false);

        if (autoSave)
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        result.State = EntityState.Detached;

        return result.Entity;
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        await _dbContext
            .Tenants
            .AddRangeAsync(entities, cancellationToken)
            .ConfigureAwait(false);

        if (autoSave)
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task DeleteAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        var existing = await _dbContext
            .Tenants
            .Where(ti => ti.Identifier == entity.Identifier)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken)
                ?? throw new MultiTenantException($"There is no tenant with the identifier '{entity.Identifier}'");

        try
        {
            if (entity is ISoftDelete softDeleteEntity)
            {
                softDeleteEntity.IsDeleted = true;
                _dbContext.Tenants.Update(entity);
            }
            else
                _dbContext.Tenants.Remove(entity);

            if (autoSave)
                await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new MultiTenantException($"Could not delete tenant with identifier '{entity.Identifier}'", ex);
        }
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        _dbContext
            .Tenants.RemoveRange(entities);

        if (autoSave)
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual Task<List<T>> GetAllAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return _dbContext
            .Tenants
            .IncludeDetails(includeDetails)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public virtual Task<T> GetAsync(KeyType id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return _dbContext
            .Tenants
            .IncludeDetails(includeDetails)
            .AsNoTracking()
            .Where(ti => ti.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual Task<T> GetByIdentifierAsync(string identifier, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return _dbContext
            .Tenants
            .AsNoTracking()
            .Where(ti => ti.Identifier == identifier)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext
            .Tenants
            .LongCountAsync(cancellationToken);
    }

    public virtual Task<T> GetHostAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        return _dbContext
            .Tenants
            .AsNoTracking()
            .Where(ti => ti.Type == TenantType.Host)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task<T> UpdateAsync(T tenant, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        var result = _dbContext.Tenants.Update(tenant);

        if (autoSave)
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        result.State = EntityState.Detached;

        return result.Entity;
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        _dbContext.Tenants.UpdateRange(entities);

        if (autoSave)
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}
