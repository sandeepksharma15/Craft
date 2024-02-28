using Craft.Domain.Contracts;

namespace Craft.Security.Contracts;

public interface ICraftRole<TKey> : IEntity<TKey>, ISoftDelete, IHasConcurrency, IHasActive, IModel<TKey>
{
    public string Description { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
}

public interface ICraftRole : ICraftRole<KeyType>, IEntity, IModel;
