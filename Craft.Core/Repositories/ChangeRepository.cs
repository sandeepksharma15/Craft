using Craft.Domain.Contracts;
using Craft.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Craft.Core.Repositories;

public class ChangeRepository<T, TKey>(DbContext appDbContext, ILogger<ChangeRepository<T, TKey>> logger)
    : ReadRepository<T, TKey>(appDbContext, logger), IChangeRepository<T, TKey> where T : class, IEntity<TKey>, new()
{
    public virtual async Task<T> AddAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[ChangeRepository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"AddAsync\"] Id: [\"{entity.Id}\"]");

        var result = await _dbSet
            .AddAsync(entity, cancellationToken)
            .ConfigureAwait(false);

        if (autoSave)
            await _appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        result.State = EntityState.Detached;

        return result.Entity;
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[ChangeRepository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"AddRangeAsync\"]");

        await _dbSet
            .AddRangeAsync(entities, cancellationToken)
            .ConfigureAwait(false);

        if (autoSave)
            await _appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task DeleteAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[ChangeRepository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"DeleteAsync\"] Id: [\"{entity.Id}\"]");

        if (entity is ISoftDelete softDeleteEntity)
        {
            softDeleteEntity.IsDeleted = true;
            _dbSet.Update(entity);
        }
        else
            _dbSet.Remove(entity);

        if (autoSave)
            await _appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task DeleteRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[ChangeRepository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"deleteRangeAsync\"]");

        if (entities.Any(entity => entity is ISoftDelete))
        {
            foreach (var entity in entities)
            {
                ISoftDelete softDeleteEntity = (ISoftDelete)entity;
                softDeleteEntity.IsDeleted = true;
            }

            _dbSet.UpdateRange(entities);
        }
        else
            _dbSet.RemoveRange(entities);

        if (autoSave)
            await _appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _appDbContext.SaveChangesAsync(cancellationToken);
    }

    public virtual async Task<T> UpdateAsync(T entity, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[ChangeRepository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"UpdateAsync\"] Id: [\"{entity.Id}\"]");

        var result = _dbSet.Update(entity);

        if (autoSave)
            await _appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        result.State = EntityState.Detached;

        return result.Entity;
    }

    public virtual async Task UpdateRangeAsync(IEnumerable<T> entities, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[ChangeRepository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"UpdateRangeAsync\"]");

        _dbSet.UpdateRange(entities);

        if (autoSave)
            await _appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }
}

public class ModificationRepository<T>(DbContext appDbContext, ILogger<ChangeRepository<T, KeyType>> logger)
    : ChangeRepository<T, KeyType>(appDbContext, logger), IChangeRepository<T>
        where T : class, IEntity, new() { }
