using Craft.MultiTenant.Contracts;

namespace Craft.MultiTenant.Options;

public class InMemoryStoreOptions<T> where T : class, ITenant, new()
{
    public static string SectionName { get; } = "MultiTenancy:InMemoryStore";

    public bool IsCaseSensitive { get; set; } = false;
    public IList<T> Tenants { get; set; } = [];
}
