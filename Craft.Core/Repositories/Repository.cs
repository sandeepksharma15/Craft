using Craft.Domain.Contracts;
using Craft.Domain.Helpers;
using Craft.Domain.Repositories;
using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Craft.Core.Repositories;

public class Repository<T, TKey>(DbContext appDbContext, ILogger<Repository<T, TKey>> logger)
    : ChangeRepository<T, TKey>(appDbContext, logger), IRepository<T, TKey> where T : class, IEntity<TKey>, new()
{
    public virtual async Task DeleteAsync(IQuery<T> query, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[Repository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"DeleteAsync\"]");

        await _dbSet
            .WithQuery(query)
            .ForEachAsync(entity =>
            {
                if (entity is ISoftDelete softDeleteEntity)
                {
                    softDeleteEntity.IsDeleted = true;
                    _dbSet.Update(entity);
                }
                else
                    _dbSet.Remove(entity);
            }, cancellationToken);

        if (autoSave)
            await _appDbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async Task<T> GetAsync(IQuery<T> query, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[Repository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetAsync\"]");

        return await _dbSet
            .WithQuery(query)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<TResult> GetAsync<TResult>(IQuery<T, TResult> query, CancellationToken cancellationToken = default)
        where TResult : class, new()
    {
        _logger.LogDebug($"[Repository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetAsync\"]");

        return await _dbSet
            .WithQuery(query)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public virtual async Task<long> GetCountAsync(IQuery<T> query, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[Repository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetCountAsync\"]");

        return await _dbSet
            .WithQuery(query)
            .LongCountAsync(cancellationToken);
    }

    public virtual async Task<PageResponse<T>> GetPagedListAsync(IQuery<T> query, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[Repository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetPagedListAsync\"]");

        var items = await _dbSet
            .WithQuery(query)
            .ToListAsync(cancellationToken);

        var totalCount = await _dbSet
            .WithQuery(query)
            .CountAsync(cancellationToken);

        int pageSize = query.Take.Value;
        int page = (query.Skip.Value / pageSize) + 1 ;

        return new PageResponse<T>(items, totalCount, page, pageSize);
    }

    public virtual async Task<PageResponse<TResult>> GetPagedListAsync<TResult>(IQuery<T, TResult> query, CancellationToken cancellationToken = default)
        where TResult : class, new()
    {
        _logger.LogDebug($"[Repository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetPagedListAsync\"]");

        var items = await _dbSet
            .WithQuery(query)
            .ToListAsync(cancellationToken);

        var totalCount = await _dbSet
            .WithQuery(query)
            .CountAsync(cancellationToken);

        int pageSize = query.Take.Value;
        int page = (query.Skip.Value / pageSize) + 1;

        return new PageResponse<TResult>(items, totalCount, page, pageSize);
    }
}

public class Repository<T>(DbContext appDbContext, ILogger<Repository<T, KeyType>> logger)
    : Repository<T, KeyType>(appDbContext, logger), IRepository<T>
        where T : class, IEntity, new() { }
