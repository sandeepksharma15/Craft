using Craft.Auditing.Attributes;
using Craft.Domain.Base;
using FluentAssertions;

namespace Auditing.Tests;

public class DisableAuditingAttributeTests
{
    private static bool HasDisableAuditingAttribute(Type type)
    {
        return type.IsDefined(typeof(DisableAuditingAttribute), inherit: false);
    }

    [Fact]
    public void DisableAuditingAttribute_ShouldBeAppliedToBaseClass()
    {
        // Arrange
        var baseClassType = typeof(BaseClass);

        // Act
        var hasAttribute = HasDisableAuditingAttribute(baseClassType);

        // Assert
        hasAttribute.Should().BeTrue();
    }

    [Fact]
    public void DisableAuditingAttribute_ShouldBeAppliedToDerivedClass()
    {
        // Arrange
        var derivedClassType = typeof(EntityDerivedClass);

        // Act
        var hasAttribute = HasDisableAuditingAttribute(derivedClassType);

        // Assert
        hasAttribute.Should().BeTrue();
    }

    [Fact]
    public void DisableAuditingAttribute_ShouldNotBeAppliedToDerivedClass()
    {
        // Arrange
        var derivedClassType = typeof(DerivedClass);

        // Act
        var hasAttribute = HasDisableAuditingAttribute(derivedClassType);

        // Assert
        hasAttribute.Should().BeFalse();
    }

    //[Fact]
    //public void DisableAuditingAttribute_ShouldThrowException_WhenAppliedToNonDerivedClass()
    //{
    //    // Arrange
    //    var nonDerivedClassType = typeof(NotDerivedClass);

    //    // Act & Assert
    //    Assert.Throws<InvalidOperationException>(() => HasDisableAuditingAttribute(nonDerivedClassType));
    //}
    [DisableAuditing]
    private class BaseClass
    {
        // Base class implementation
    }

    private class DerivedClass : BaseClass
    {
        // Derived class implementation
    }

    [DisableAuditing]
    private class EntityDerivedClass : EntityBase
    {
        // Derived class implementation
    }

    [DisableAuditing]
    private class NotDerivedClass
    {
        // Not derived from Entity
    }
}
