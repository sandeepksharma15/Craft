using System.Net.Http.Json;
using Craft.Domain.Contracts;
using Craft.Domain.HttpServices;
using Microsoft.Extensions.Logging;

namespace Craft.Core.HttpServices;

public class HttpReadService<T, TKey>(Uri apiURL, HttpClient httpClient, ILogger<HttpReadService<T, TKey>> logger)
    : IHttpReadService<T, TKey> where T : class, IEntity<TKey>, IModel<TKey>
{
    protected readonly Uri _apiURL = apiURL;
    protected readonly HttpClient _httpClient = httpClient;
    protected readonly ILogger _logger = logger;

    public virtual async Task<List<T>> GetAllAsync(bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[HttpReadService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetAllAsync\"]");

        HttpResponseMessage response = await _httpClient
            .GetAsync(new Uri($"{_apiURL}/{includeDetails}"), cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        return await response
            .Content
            .ReadFromJsonAsync<List<T>>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public virtual async Task<T> GetAsync(TKey id, bool includeDetails = false, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[HttpReadService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetAsync\"]");

        HttpResponseMessage response = await _httpClient
            .GetAsync(new Uri($"{_apiURL}/{id}/{includeDetails}"), cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        return await response
            .Content
            .ReadFromJsonAsync<T>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public virtual async Task<long> GetCountAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[HttpReadService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetCountAsync\"]");

        HttpResponseMessage response = await _httpClient
            .GetAsync(new Uri($"{_apiURL}/count"), cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        return await response
            .Content
            .ReadFromJsonAsync<long>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}

public class HttpReadService<T>(Uri apiURL, HttpClient httpClient, ILogger<HttpReadService<T>> logger)
    : HttpReadService<T, KeyType>(apiURL, httpClient, logger), IHttpReadService<T>
        where T : class, IEntity, IModel { }
