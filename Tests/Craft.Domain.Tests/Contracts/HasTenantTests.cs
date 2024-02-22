using Craft.Domain.Contracts;
using FluentAssertions;

namespace Craft.Domain.Tests.Contracts;

public class HasTenantTests
{
    [Fact]
    public void IHasTenant_ColumnName_IsConstant()
    {
        // Assert
        IHasTenant.ColumnName.Should().Be("TenantId");
    }

    [Fact]
    public void IHasTenant_CastsToIHasTenantOfConcreteType()
    {
        // Arrange
        ConcreteHasTenant instance = new();

        // Act & Assert
        IHasTenant castInstance = instance;
        castInstance.Should().NotBeNull();
    }

    [Fact]
    public void IHasTenantOfTKey_GetTenantId_ReturnsTenantIdValue()
    {
        // Arrange
        ConcreteHasTenant instance = new() { TenantId = 123 };
        IHasTenant castInstance = instance;

        // Act & Assert
        var actualId = castInstance.GetTenantId();
        actualId.Should().Be(123);
    }

    [Fact]
    public void IHasTenantOfTKey_SetTenantId_SetsTenantIdValue()
    {
        // Arrange
        ConcreteHasTenant instance = new() { TenantId = 123 };
        IHasTenant castInstance = instance;

        // Act
        castInstance.SetTenantId(456);

        // Assert
        instance.TenantId.Should().Be(456);
    }

    [Fact]
    public void IHasTenantOfTKey_IsTenantIdSet_ReturnsTrueForSetTenantId()
    {
        // Arrange
        IHasTenant castInstance = (ConcreteHasTenant)new() { TenantId = 123 };

        // Act & Assert
        bool isSet = castInstance.IsTenantIdSet();
        isSet.Should().BeTrue();
    }

    [Fact]
    public void IHasTenantOfTKey_IsTenantIdSet_ReturnsFalseForDefaultTenantId()
    {
        // Arrange
        IHasTenant castInstance = (ConcreteHasTenant)new();

        // Act & Assert
        bool isSet = castInstance.IsTenantIdSet();
        isSet.Should().BeFalse();
    }

    private class ConcreteHasTenant : IHasTenant
    {
        public KeyType Id { get; set; }
        public KeyType TenantId { get; set; }
    }
}
