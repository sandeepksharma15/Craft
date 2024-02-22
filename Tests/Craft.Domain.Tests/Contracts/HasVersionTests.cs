using Craft.Domain.Contracts;
using FluentAssertions;

namespace Craft.Domain.Tests.Contracts;

public class HasVersionTests
{
    [Fact]
    public void IHasVersion_ColumnName_IsConstant()
    {
        // Assert
        IHasVersion.ColumnName.Should().Be("Version");
    }

    [Fact]
    public void GetVersion_ReturnsInitialVersion()
    {
        // Arrange
        // Act
        IHasVersion hasVersion = (ConcreteHasVersion)new();
        long actualVersion = hasVersion.GetVersion();

        // Assert
        actualVersion.Should().Be(0); // Default initial value
    }

    [Fact]
    public void SetVersion_SetsVersionValue()
    {
        // Arrange
        ConcreteHasVersion instance = new();
        const long expectedVersion = 123;

        // Act
        IHasVersion hasVersion = instance;
        hasVersion.SetVersion(expectedVersion);

        // Assert
        instance.Version.Should().Be(expectedVersion);
    }

    [Fact]
    public void IncrementVersion_IncreasesVersionByOne()
    {
        // Arrange
        ConcreteHasVersion instance = new() { Version = 10 };

        // Act
        IHasVersion hasVersion = instance;
        hasVersion.IncrementVersion();

        // Assert
        instance.Version.Should().Be(11);
    }

    [Fact]
    public void DecrementVersion_DecreasesVersionByOne()
    {
        // Arrange
        ConcreteHasVersion instance = new() { Version = 20 };

        // Act
        IHasVersion hasVersion = instance;
        hasVersion.DecrementVersion();

        // Assert
        instance.Version.Should().Be(19);
    }

    [Fact]
    public void DecrementVersion_DoesNotGoBelowZero()
    {
        // Arrange
        ConcreteHasVersion instance = new() { Version = 0 };

        // Act
        IHasVersion hasVersion = instance;
        hasVersion.DecrementVersion();

        // Assert
        instance.Version.Should().Be(0);
    }

    private class ConcreteHasVersion : IHasVersion
    {
        public long Version { get; set; }
    }
}
