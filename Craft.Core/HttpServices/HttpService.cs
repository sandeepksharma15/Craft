using System.Net.Http.Json;
using Craft.Domain.Contracts;
using Craft.Domain.Helpers;
using Craft.Domain.HttpServices;
using Craft.QuerySpec.Contracts;
using Microsoft.Extensions.Logging;

namespace Craft.Core.HttpServices;

public class HttpService<T, ViewT, DataTransferT, TKey>(Uri apiURL, HttpClient httpClient, ILogger<HttpService<T, ViewT, DataTransferT, TKey>> logger)
    : HttpChangeService<T, ViewT, DataTransferT, TKey>(apiURL, httpClient, logger), IHttpService<T, ViewT, DataTransferT, TKey>
        where T : class, IEntity<TKey>, IModel<TKey>
        where ViewT : class, IModel<TKey>
        where DataTransferT : class, IModel<TKey>
{
    public async Task<HttpResponseMessage> DeleteAsync(IQuery<T> query, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[HttpService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"DeleteAsync\"]");

        return await _httpClient
            .PostAsJsonAsync(new Uri($"{_apiURL}/delete"), query, cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<T> GetAsync(IQuery<T> query, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[HttpService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetAsync\"]");

        HttpResponseMessage response = await _httpClient
            .PostAsJsonAsync(new Uri($"{_apiURL}/find"), query, cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        return await response
            .Content
            .ReadFromJsonAsync<T>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<TResult> GetAsync<TResult>(IQuery<T, TResult> query, CancellationToken cancellationToken = default)
        where TResult : class, new()
    {
        _logger.LogDebug($"[HttpService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetAsync\"]");

        HttpResponseMessage response = await _httpClient
            .PostAsJsonAsync(new Uri($"{_apiURL}/findone"), query, cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        return await response
            .Content
            .ReadFromJsonAsync<TResult>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<long> GetCountAsync(IQuery<T> query, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[HttpService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetCountAsync\"]");

        HttpResponseMessage response = await _httpClient
            .PostAsJsonAsync(new Uri($"{_apiURL}/filtercount"), query, cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        return await response
            .Content
            .ReadFromJsonAsync<long>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<PageResponse<T>> GetPagedListAsync(IQuery<T> query, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[HttpService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetPagedListAsync\"]");

        HttpResponseMessage response = await _httpClient
            .PostAsJsonAsync(new Uri($"{_apiURL}/search"), query, cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        return await response
            .Content
            .ReadFromJsonAsync<PageResponse<T>>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }

    public async Task<PageResponse<TResult>> GetPagedListAsync<TResult>(IQuery<T, TResult> query, CancellationToken cancellationToken = default)
        where TResult : class, new()
    {
        _logger.LogDebug($"[HttpService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetPagedListAsync\"]");

        HttpResponseMessage response = await _httpClient
            .PostAsJsonAsync(new Uri($"{_apiURL}/select"), query, cancellationToken)
            .ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        return await response
            .Content
            .ReadFromJsonAsync<PageResponse<TResult>>(cancellationToken: cancellationToken)
            .ConfigureAwait(false);
    }
}

public class HttpService<T, ViewT, DataTransferT>(Uri apiURL, HttpClient httpClient, ILogger<HttpService<T, ViewT, DataTransferT>> logger)
    : HttpService<T, ViewT, DataTransferT, KeyType>(apiURL, httpClient, logger), IHttpService<T, ViewT, DataTransferT>
        where T : class, IEntity, IModel
        where ViewT : class, IModel
        where DataTransferT : class, IModel { }

public class HttpService<T>(Uri apiURL, HttpClient httpClient, ILogger<HttpService<T>> logger)
    : HttpService<T, T, T, KeyType>(apiURL, httpClient, logger), IHttpService<T>
        where T : class, IEntity, IModel { }
