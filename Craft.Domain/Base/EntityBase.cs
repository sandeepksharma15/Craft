using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Craft.Domain.Contracts;

namespace Craft.Domain.Base;

[Serializable]
public abstract class EntityBase : EntityBase<KeyType>, IEntity, IModel
{
    protected EntityBase() { }

    protected EntityBase(KeyType id) : base(id) { }
}

[Serializable]
public abstract class EntityBase<TKey> : IEntity<TKey>, IHasConcurrency, ISoftDelete, IModel<TKey>
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column(Order = 0)]
    public virtual TKey Id { get; set; }

    [ConcurrencyCheck]
    [MaxLength(IHasConcurrency.MaxLength)]
    public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

    public virtual bool IsDeleted { get; set; }

    protected EntityBase() { }

    protected EntityBase(TKey id) { Id = id; }

    public static bool operator !=(EntityBase<TKey> left, EntityBase<TKey> right)
        => !Equals(left, right);

    public static bool operator ==(EntityBase<TKey> left, EntityBase<TKey> right)
        => Equals(left, right);

    public override bool Equals(object obj)
    {
        if (obj is not EntityBase<TKey>)
            return false;
        if (ReferenceEquals(this, obj))
            return true;
        if (GetType() != obj.GetType())
            return false;

        // Different tenants may have an entity with same Id.
        if (this is IHasTenant self && obj is IHasTenant other && self.TenantId != other.TenantId)
            return false;

        return Equals(Id, ((EntityBase<TKey>)obj).Id);
    }

    public override int GetHashCode()
        => 2108858624 + EqualityComparer<TKey>.Default.GetHashCode(Id);

    public virtual bool IsNew()
        => EqualityComparer<TKey>.Default.Equals(Id, default);

    public override string ToString()
        => $"[ENTITY: {GetType().Name}] Key = {Id}";
}
