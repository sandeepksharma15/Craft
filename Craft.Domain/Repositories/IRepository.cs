using Craft.Domain.Contracts;
using Craft.Domain.Helpers;
using Craft.QuerySpec.Contracts;

namespace Craft.Domain.Repositories;

public interface IRepository;

public interface IRepository<T, TKey> : IChangeRepository<T, TKey> where T : class, IEntity<TKey>, new()
{
    /// <summary>
    /// Deletes all the entities that meet the criteria by the given <paramref name="query"/>
    /// </summary>
    /// <param name="query">A Query containing filtering parameters</param>
    /// <param name="autoSave">Set false to save changes later calling SaveChangesAsync Manually</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task DeleteAsync(IQuery<T> query, bool autoSave = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of all the entities that meet the criteria by the given <paramref name="query"/>
    /// </summary>
    /// <param name="query">A Query containing filtering and sorting parameters</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>List of entities</returns>
    async Task<List<T>> GetAllAsync(IQuery<T> query, bool includeDetails = false,
        CancellationToken cancellationToken = default)
        => (await GetPagedListAsync(query, 1, int.MaxValue, includeDetails, cancellationToken))
            .Items
            .ToList();

    /// <summary>
    /// Gets a list of all the entities that meet the criteria by the given <paramref name="query"/>
    /// </summary>
    /// <param name="query">A Query containing filtering and sorting parameters</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>List of entities</returns>
    async Task<List<TResult>> GetAllAsync<TResult>(IQuery<T, TResult> query, bool includeDetails = false,
        CancellationToken cancellationToken = default) where TResult : class, new()
        => (await GetPagedListAsync<TResult>(query, 1, int.MaxValue, includeDetails, cancellationToken))
            .Items
            .ToList();

    /// <summary>
    /// Get a single entity by the given <paramref name="query"/>; returns null if no entry meets criteria
    /// </summary>
    /// <param name="query">
    ///     A Query containing filtering parameters
    ///     It throws <see cref="InvalidOperationException"/> if there are multiple entities with the given <paramref name="predicate"/>.
    /// </param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>The entity</returns>
    Task<T> GetAsync(IQuery<T> query, bool includeDetails = true, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get TResult by the given <paramref name="query"/>; returns null if no entry meets criteria
    /// </summary>
    /// <param name="query">
    ///     A Query containing filtering parameters
    ///     It throws <see cref="InvalidOperationException"/> if there are multiple entities with the given <paramref name="predicate"/>.
    /// </param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>TResult</returns>
    Task<TResult> GetAsync<TResult>(IQuery<T, TResult> query, bool includeDetails = true,
        CancellationToken cancellationToken = default) where TResult : class, new();

    Task<long> GetCountAsync(IQuery<T> specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a paginated list of entities.
    /// </summary>
    /// <param name="query">A Query containing filtering and sorting parameters</param>
    /// <param name="page">The page for which the data is desired</param>
    /// <param name="pageSize">The number of entities required per page</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Paginated list of entities</returns>
    Task<PageResponse<T>> GetPagedListAsync(IQuery<T> query, int page, int pageSize, bool includeDetails = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a paginated list of entities.
    /// </summary>
    /// <param name="page">The page for which the data is desired</param>
    /// <param name="pageSize">The number of entities required per page</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Paginated list of entities</returns>
    Task<PageResponse<T>> GetPagedListAsync(int page, int pageSize, bool includeDetails = false,
        CancellationToken cancellationToken = default)
        => GetPagedListAsync(null, page, pageSize, includeDetails, cancellationToken);

    /// <summary>
    /// Gets a paginated list of TResult
    /// </summary>
    /// <param name="query">A Query containing filtering and sorting parameters</param>
    /// <param name="page">The page for which the data is desired</param>
    /// <param name="pageSize">The number of entities required per page</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Paginated list of TResult</returns>
    Task<PageResponse<TResult>> GetPagedListAsync<TResult>(IQuery<T, TResult> query, int page, int pageSize,
        bool includeDetails = false, CancellationToken cancellationToken = default)
            where TResult : class, new();
}

public interface IRepository<T> : IRepository<T, KeyType> where T : class, IEntity, new();
