using System.Text.Json;
using Craft.Domain.Contracts;
using Craft.MultiTenant.Contracts;

namespace Craft.MultiTenant.Stores;

public class RemoteApiStoreClient<T> where T : class, ITenant, IEntity, new()
{
    private readonly IHttpClientFactory _clientFactory;

    public RemoteApiStoreClient(IHttpClientFactory clientFactory)
    {
        ArgumentNullException.ThrowIfNull(clientFactory, nameof(clientFactory));

        _clientFactory = clientFactory;
    }

    public async Task<T> GetByIdentifierAsync(string endpointTemplate, string identifier)
    {
        var client = _clientFactory.CreateClient(typeof(RemoteApiStoreClient<T>).FullName!);
        var uri = endpointTemplate.Replace(RemoteApiStore<T>.EndpointIdentifierToken, identifier);
        var response = await client.GetAsync(new Uri(uri));

        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();

#pragma warning disable CA1869 // Cache and reuse 'JsonSerializerOptions' instances
        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));
#pragma warning restore CA1869 // Cache and reuse 'JsonSerializerOptions' instances
    }
}
