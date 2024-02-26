using Craft.Domain.Contracts;

namespace Craft.Domain.Helpers;

public static class EntityHelper
{
    public static bool Equals(this IEntity entity1, IEntity entity2)
        => EntityEquals(entity1, entity2);

    public static bool EntityEquals(IEntity entity1, IEntity entity2)
    {
        if (entity1 == null || entity2 == null)
            return false;

        if (ReferenceEquals(entity1, entity2))
            return true;

        // Must have a IS-A relation of types or must be same type
        Type typeOfEntity1 = entity1.GetType();
        Type typeOfEntity2 = entity2.GetType();

        if (!typeOfEntity1.IsAssignableFrom(typeOfEntity2) && !typeOfEntity2.IsAssignableFrom(typeOfEntity1))
            return false;

        // IDs should be same
        if (entity1.Id != entity2.Id)
            return false;

        // Different tenants may have an entity with same Id.
        return entity1 is not IHasTenant tenant1 || entity2 is not IHasTenant tenant2
            || tenant1.TenantId == tenant2.TenantId;
    }

    public static bool HasDefaultId<TKey>(this IEntity<TKey> entity)
    {
        if (EqualityComparer<TKey>.Default.Equals(entity.Id, default))
            return true;

        // Workaround for EF Core since it sets int/long to min value when attaching to dbcontext
        if (typeof(TKey) == typeof(int))
            return Convert.ToInt32(entity.Id) <= 0;

        if (typeof(TKey) == typeof(long))
            return Convert.ToInt64(entity.Id) <= 0;

        return false;
    }

    public static bool HasDefaultId(this IEntity entity)
        => HasDefaultId<KeyType>(entity);

    public static bool IsEntity(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return typeof(IEntity).IsAssignableFrom(type);
    }

    public static bool IsMultiTenant<TEntity>() where TEntity : IEntity
        => IsMultiTenant(typeof(TEntity));

    public static bool IsMultiTenant(this Type type)
        => typeof(IHasTenant).IsAssignableFrom(type);
}
