using Craft.Domain.Base;

namespace Craft.Auditing.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class DisableAuditingAttribute : Attribute
{
    public DisableAuditingAttribute()
    {
        if (!IsDerivedFromClass(typeof(EntityBase)))
            throw new InvalidOperationException($"The '{nameof(DisableAuditingAttribute)}' attribute can only be applied to classes derived from the 'Entity' base class.");
    }

    private static bool IsDerivedFromClass(Type baseClassType)
    {
        var callingAssembly = System.Reflection.Assembly.GetCallingAssembly();

        foreach (var type in callingAssembly.GetTypes())
            if (type.BaseType != null && baseClassType.IsDerivedFromClass(type.BaseType))
                return true;

        return false;
    }
}
