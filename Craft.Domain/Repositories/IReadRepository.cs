using Craft.Domain.Contracts;

namespace Craft.Domain.Repositories;

public interface IReadRepository<T, TKey> : ICoreRepository<T, TKey> where T : class, IEntity<TKey>, new()
{
    /// <summary>
    /// Gets a list of all the entities.
    /// </summary>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>List of entities</returns>
    Task<List<T>> GetAllAsync(bool includeDetails = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an entity with given primary key.
    /// </summary>
    /// <param name="id">Primary key of the entity; if not found, returns null</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Entity</returns>
    Task<T> GetAsync(TKey id, bool includeDetails = false, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets total count of all entities.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>total count of entities</returns>
    Task<long> GetCountAsync(CancellationToken cancellationToken = default);
}

public interface IReadRepository<T> : IReadRepository<T, KeyType> where T : class, IEntity, new();
