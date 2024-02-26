using Craft.Domain.Contracts;
using Craft.Domain.Helpers;
using Craft.QuerySpec.Contracts;

namespace Craft.Domain.HttpServices;

public interface IHttpService;

public interface IHttpService<T, ViewT, DataTransferT, TKey> : IChangeHttpService<T, ViewT, DataTransferT, TKey>
    where T : class, IEntity<TKey>, IModel<TKey>
    where ViewT : class, IModel<TKey>
    where DataTransferT : class, IModel<TKey>

{
    /// <summary>
    /// Deletes all the entities that meet the criteria by the given <paramref name="query"/>
    /// </summary>
    /// <param name="query">A Query containing filtering parameters</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    Task<HttpResponseMessage> DeleteAsync(IQuery<T> query, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a list of all the entities that meet the criteria by the given <paramref name="query"/>
    /// </summary>
    /// <param name="query">A Query containing filtering and sorting parameters</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>List of entities</returns>
    async Task<List<T>> GetAllAsync(IQuery<T> query,
        bool includeDetails = false,
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
    async Task<List<TResult>> GetAllAsync<TResult>(IQuery<T, TResult> query,
        bool includeDetails = false,
        CancellationToken cancellationToken = default) where TResult : class, new()
        => (await GetPagedListAsync(query, 1, int.MaxValue, includeDetails, cancellationToken))
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
    Task<T> GetAsync(IQuery<T> query,
        bool includeDetails = true,
        CancellationToken cancellationToken = default);

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
    Task<TResult> GetAsync<TResult>(IQuery<T, TResult> query,
        bool includeDetails = true,
        CancellationToken cancellationToken = default) where TResult : class, new();

    /// <summary>
    /// Gets total count of all entities that meet the criteria by the given <paramref name="specification"/>
    /// </summary>
    /// <param name="specification">A Query containing filtering parameters</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>total count of entities</returns>
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
    Task<PageResponse<T>> GetPagedListAsync(IQuery<T> query,
        int page,
        int pageSize,
        bool includeDetails = false,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a paginated list of entities.
    /// </summary>
    /// <param name="page">The page for which the data is desired</param>
    /// <param name="pageSize">The number of entities required per page</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Paginated list of entities</returns>
    Task<PageResponse<T>> GetPagedListAsync(int page,
        int pageSize,
        bool includeDetails = false,
        CancellationToken cancellationToken = default)
            => GetPagedListAsync(null, page, pageSize, includeDetails, cancellationToken);

    /// <summary>
    /// Gets a paginated list of entities.
    /// </summary>
    /// <param name="page">The page for which the data is desired</param>
    /// <param name="pageSize">The number of entities required per page</param>
    /// <param name="includeDetails">Set true to include all children of this entity</param>
    /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>Paginated list of entities</returns>
    Task<PageResponse<TResult>> GetPagedListAsync<TResult>(IQuery<T, TResult> query,
        int page,
        int pageSize,
        bool includeDetails = false,
        CancellationToken cancellationToken = default) where TResult : class, new();
}

public interface IHttpService<T, ViewT, DataTransferT> : IHttpService<T, ViewT, DataTransferT, KeyType>
    where T : class, IEntity, IModel
    where ViewT : class, IModel
    where DataTransferT : class, IModel;

public interface IHttpService<T> : IHttpService<T, T, T>
    where T : class, IEntity, IModel;
