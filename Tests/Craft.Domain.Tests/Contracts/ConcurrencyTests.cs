using Craft.Domain.Contracts;
using FluentAssertions;
using Xunit;

namespace Craft.Domain.Tests.Contracts;

public class ConcurrencyTests
{
    [Fact]
    public void GetConcurrencyStamp_Should_ReturnStamp()
    {
        // Arrange
        IHasConcurrency entity = new TestEntity();
        entity.SetConcurrencyStamp("test-stamp");

        // Act
        string stamp = entity.GetConcurrencyStamp();

        // Assert
        stamp.Should().Be("test-stamp");
    }

    [Fact]
    public void HasConcurrencyStamp_WithoutStamp_Should_ReturnFalse()
    {
        // Arrange
        IHasConcurrency entity = new TestEntity();

        // Act
        bool hasStamp = entity.HasConcurrencyStamp();

        // Assert
        hasStamp.Should().BeFalse();
    }

    [Fact]
    public void HasConcurrencyStamp_WithStamp_Should_ReturnTrue()
    {
        // Arrange
        IHasConcurrency entity = new TestEntity();
        entity.SetConcurrencyStamp("test-stamp");

        // Act
        bool hasStamp = entity.HasConcurrencyStamp();

        // Assert
        hasStamp.Should().BeTrue();
    }

    [Fact]
    public void SetConcurrencyStamp_Should_SetStamp()
    {
        // Arrange
        IHasConcurrency entity = new TestEntity();

        // Act
        entity.SetConcurrencyStamp("test-stamp");

        // Assert
        entity.ConcurrencyStamp.Should().Be("test-stamp");
    }

    [Fact]
    public void SetConcurrencyStamp_WithNull_Should_GenerateNewStamp()
    {
        // Arrange
        IHasConcurrency entity = new TestEntity();

        // Act
        entity.SetConcurrencyStamp(null);

        // Assert
        entity.ConcurrencyStamp.Should().NotBeNullOrEmpty();
    }

    private class TestEntity : IHasConcurrency
    {
        public string ConcurrencyStamp { get; set; }
    }
}
