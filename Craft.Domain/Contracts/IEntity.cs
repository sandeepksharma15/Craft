namespace Craft.Domain.Contracts;

public interface IEntity<TKey> : IHasId<TKey>;

public interface IEntity : IEntity<KeyType>;
