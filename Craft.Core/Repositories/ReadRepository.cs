using Craft.Domain.Contracts;
using Craft.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Craft.Core.Repositories;

public class ReadRepository<T, TKey>(DbContext appDbContext, ILogger<ReadRepository<T, TKey>> logger)
    : CoreRepository<T, TKey>(appDbContext, logger), IReadRepository<T, TKey> where T : class, IEntity<TKey>, new()
{
    public virtual async Task<List<T>> GetAllAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[ReadRepository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetAllAsync\"]");

        return await _dbSet
           .IncludeDetails(includeDetails)
           .AsNoTracking()
           .ToListAsync(cancellationToken)
           .ConfigureAwait(false);
    }

    public virtual async Task<T> GetAsync(TKey id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[ReadRepository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetAsync\"] Id: [\"{id}\"]");

        return await _dbSet
            .IncludeDetails(includeDetails)
            .AsNoTracking()
            .Where(ti => ti.Id.Equals(id))
            .FirstOrDefaultAsync(cancellationToken)
            .ConfigureAwait(false);
    }

    public virtual async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[ReadRepository] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetCountAsync\"]");

        return await _dbSet
            .LongCountAsync(cancellationToken)
            .ConfigureAwait(false);
    }
}

public class ReadRepository<T>(DbContext appDbContext, ILogger<ReadRepository<T, KeyType>> logger)
    : ReadRepository<T, KeyType>(appDbContext, logger), IReadRepository<T>
        where T : class, IEntity, new() { }
