using Craft.Domain.Contracts;
using FluentAssertions;

namespace Craft.Domain.Tests.Contracts;

public class HasUserTests
{
    [Fact]
    public void IHasUser_ColumnName_IsConstant()
    {
        // Assert
        IHasUser.ColumnName.Should().Be("UserId");
    }

    [Fact]
    public void IHasUser_CastsToIHasUserOfConcreteType()
    {
        // Arrange
        // Act & Assert
        IHasUser castInstance = (ConcreteHasUser)new();
        castInstance.Should().NotBeNull();
    }

    [Fact]
    public void IHasUserOfTKey_GetUserId_ReturnsUserIdValue()
    {
        // Arrange
        // Act & Assert
        IHasUser hasUser = (ConcreteHasUser)new() { UserId = 123 };
        var actualId = hasUser.GetUserId();
        actualId.Should().Be(123);
    }

    [Fact]
    public void IHasUserOfTKey_SetUserId_SetsUserIdValue()
    {
        // Arrange
        ConcreteHasUser instance = new() { UserId = 123 };

        // Act
        IHasUser hasUser = instance;
        hasUser.SetUserId(456);

        // Assert
        instance.UserId.Should().Be(456);
    }

    [Fact]
    public void IHasUserOfTKey_IsUserIdSet_ReturnsTrueForSetUserId()
    {
        // Arrange
        // Act & Assert
        IHasUser hasUser = (ConcreteHasUser)new() { UserId = 123 };
        bool isSet = hasUser.IsUserIdSet();
        isSet.Should().BeTrue();
    }

    [Fact]
    public void IHasUserOfTKey_IsUserIdSet_ReturnsFalseForDefaultUserId()
    {
        // Arrange
        // Act & Assert
        IHasUser hasUser = (ConcreteHasUser)new();
        bool isSet = hasUser.IsUserIdSet();
        isSet.Should().BeFalse();
    }

    private class ConcreteHasUser : IHasUser
    {
        public KeyType UserId { get; set; }
    }
}
