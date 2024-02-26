using Craft.Domain.Contracts;

namespace Craft.Domain.Base;

[Serializable]
public abstract class DtoBase : DtoBase<KeyType>, IModel;

[Serializable]
public abstract class DtoBase<TKey> : IHasConcurrency, ISoftDelete, IModel<TKey>
{
    public virtual TKey Id { get; set; }

    public virtual string ConcurrencyStamp { get; set; }
    public virtual bool IsDeleted { get; set; }
}
