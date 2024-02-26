using Craft.Domain.Contracts;

namespace Craft.Domain.Base;

[Serializable]
public abstract class VmBase : VmBase<KeyType>, IModel;

[Serializable]
public abstract class VmBase<TKey> : IHasConcurrency, ISoftDelete, IModel<TKey>
{
    public virtual string ConcurrencyStamp { get; set; }
    public virtual TKey Id { get; set; }
    public virtual bool IsDeleted { get; set; }
}
