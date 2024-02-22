using Craft.Domain.Contracts;
using FluentAssertions;
using Xunit;

namespace Craft.Domain.Tests.Contracts;

public class ActiveTests
{
    [Fact]
    public void Activate_Should_SetIsActiveToTrue()
    {
        // Arrange
        IHasActive entity = new TestEntity();

        // Act
        entity.Activate();

        // Assert
        entity.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Deactivate_Should_SetIsActiveToFalse()
    {
        // Arrange
        IHasActive entity = new TestEntity();
        entity.Activate(); // Set IsActive to true initially

        // Act
        entity.Deactivate();

        // Assert
        entity.IsActive.Should().BeFalse();
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void SetActive_Should_SetIsActive(bool isActive)
    {
        // Arrange
        IHasActive entity = new TestEntity();

        // Act
        entity.SetActive(isActive);

        // Assert
        entity.IsActive.Should().Be(isActive);
    }

    [Fact]
    public void ToggleActive_Should_InvertIsActive()
    {
        // Arrange
        IHasActive entity = new TestEntity();
        entity.Activate(); // Set IsActive to true initially

        // Act
        entity.ToggleActive();

        // Assert
        entity.IsActive.Should().BeFalse();

        // Act again
        entity.ToggleActive();

        // Assert again
        entity.IsActive.Should().BeTrue();
    }

    private class TestEntity : IHasActive
    {
        public bool IsActive { get; set; }
    }
}
