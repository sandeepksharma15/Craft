using Craft.Auditing.Attributes;
using Craft.Auditing.Extensions;
using FluentAssertions;

namespace Auditing.Tests;

public class TypeExtensionsTests
{
    [Fact]
    public void HasDisableAuditingAttribute_Should_Return_False_When_DisableAuditingAttribute_Is_Not_Present()
    {
        // Arrange
        var type = typeof(TestClassWithoutDisableAuditingAttribute);

        // Act
        var result = type.HasAuditAttributeDisabled();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HasDisableAuditingAttribute_Should_Return_True_When_DisableAuditingAttribute_Is_Present()
    {
        // Arrange
        var type = typeof(TestClassWithDisableAuditingAttribute);

        // Act
        var result = type.HasAuditAttributeDisabled();

        // Assert
        result.Should().BeTrue();
    }

    [DisableAuditing]
    private class TestClassWithDisableAuditingAttribute;

    private class TestClassWithoutDisableAuditingAttribute;
}
