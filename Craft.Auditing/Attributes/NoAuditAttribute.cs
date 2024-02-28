using System.Runtime.CompilerServices;
using Craft.Domain.Base;

namespace Craft.Auditing.Attributes;

/// <summary>
/// Disables auditing for the decorated class. This attribute can only be applied to classes derived from a specific base class.
/// </summary>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class NoAuditAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NoAuditAttribute"/> class.
    /// </summary>
    //public NoAuditAttribute(Type targetClassType)
    //{
    //    //if (targetClassType.IsDerivedFromClass(typeof(EntityBase)))
    //    //    throw new InvalidOperationException($"The '{nameof(NoAuditAttribute)}' attribute can only be applied to classes derived from the 'Entity' base class.");
    //}
}
