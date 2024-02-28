using Craft.Auditing.Attributes;

namespace Craft.Auditing.Extensions;

public static class TypeExtensions
{
    public static bool HasNoAuditAttribute(this Type type)
        => type.IsDefined(typeof(NoAuditAttribute), inherit: false);
}
