using Craft.Domain.Contracts;
using FluentAssertions;
using Xunit;

namespace Craft.Domain.Tests.Contracts;

public class HasIdTests
{
    [Fact]
    public void IHasId_IsNew_ReturnsTrueForDefaultId()
    {
        // Arrange
        IHasId instance = new ConcreteHasId();

        // Act & Assert
        instance.IsNew.Should().BeTrue();
    }

    [Fact]
    public void IHasId_IsNew_ReturnsFalseForNonDefaultId()
    {
        // Arrange
        IHasId instance = new ConcreteHasId { Id = 123 };

        // Act & Assert
        instance.IsNew.Should().BeFalse();
    }

    [Fact]
    public void IHasId_GetId_ReturnsIdValue()
    {
        // Arrange
        IHasId instance = new ConcreteHasId { Id = 456 };

        // Act & Assert
        KeyType actualId = instance.GetId();
        actualId.Should().Be(456);
    }

    [Fact]
    public void IHasId_SetId_SetsIdValue()
    {
        // Arrange
        IHasId instance = new ConcreteHasId();

        // Act
        instance.SetId(789);

        // Assert
        instance.Id.Should().Be(789);
    }

    [Fact]
    public void IHasId_ColumnName_IsConstant()
    {
        // Assert
        IHasId.ColumnName.Should().Be("Id");
    }

    [Fact]
    public void IHasId_CastsToIHasIdOfConcreteType()
    {
        // Arrange
        ConcreteHasId instance = new();

        // Act & Assert
        IHasId castInstance = instance;
        castInstance.Should().NotBeNull();
    }

    private class ConcreteHasId : IHasId
    {
        public KeyType Id { get; set; }
    }
}
