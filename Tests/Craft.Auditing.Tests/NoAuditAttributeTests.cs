using Craft.Auditing.Attributes;
using Craft.Auditing.Extensions;
using Craft.Domain.Base;
using FluentAssertions;

namespace Auditing.Tests;

public class NoAuditAttributeTests
{
    [Fact]
    public void NoAuditAttribute_ShouldBeAppliedToBaseClass()
    {
        // Arrange
        var baseClassType = typeof(BaseClass);

        // Act
        var hasAttribute = baseClassType.HasNoAuditAttribute();

        // Assert
        hasAttribute.Should().BeTrue();
    }

    [Fact]
    public void NoAuditAttribute_ShouldBeAppliedToDerivedClass()
    {
        // Arrange
        var derivedClassType = typeof(EntityDerivedClass);

        // Act
        var hasAttribute = derivedClassType.HasNoAuditAttribute();

        // Assert
        hasAttribute.Should().BeTrue();
    }

    [Fact]
    public void NoAuditAttribute_ShouldNotBeAppliedToDerivedClass()
    {
        // Arrange
        var derivedClassType = typeof(DerivedClass);

        // Act
        var hasAttribute = derivedClassType.HasNoAuditAttribute();

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

    [NoAudit]
    private class BaseClass
    {
        // Base class implementation
    }

    private class DerivedClass : BaseClass
    {
        // Derived class implementation
    }

    [NoAudit]
    private class EntityDerivedClass : EntityBase
    {
        // Derived class implementation
    }

    [NoAudit]
    private class NotDerivedClass
    {
        // Not derived from Entity
    }
}
