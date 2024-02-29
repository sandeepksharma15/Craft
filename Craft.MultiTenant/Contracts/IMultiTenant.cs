namespace Craft.MultiTenant.Contracts;

public interface IMultiTenant<TKey>
{
    TKey TenantId { get; set; }
}

public interface IMultiTenant : IMultiTenant<KeyType>;
