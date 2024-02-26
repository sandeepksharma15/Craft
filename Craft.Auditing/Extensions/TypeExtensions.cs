using Craft.Auditing.Attributes;

namespace Craft.Auditing.Extensions;

public static class TypeExtensions
{
    public static bool HasAuditAttributeDisabled(this Type type)
    {
        return type.IsDefined(typeof(DisableAuditingAttribute), inherit: false);
    }
}
