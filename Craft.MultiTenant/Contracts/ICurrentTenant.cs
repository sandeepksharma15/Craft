namespace Craft.MultiTenant.Contracts;

public interface ICurrentTenant<TKey> : ITenant<TKey>;

public interface ICurrentTenant : ICurrentTenant<KeyType>;
