using Craft.Utilities.Helpers;
using FluentAssertions;

namespace Craft.Utilities.Tests.Helpers;

public class TestHelpersTest
{
    [Fact]
    public void AreTheySame_SimpleObjects_SameValues_NoExceptions()
    {
        // Arrange
        var obj1 = new { Name = "Test", Age = 30 };
        var obj2 = new { Name = "Test", Age = 30 };

        // Act & Assert
        TestHelpers.AreTheySame(obj1, obj2);
    }

    [Fact]
    public void AreTheySame_SimpleObjects_DifferentValues_ThrowsAssertionError()
    {
        // Arrange
        var obj1 = new { Name = "Test", Age = 30 };
        var obj2 = new { Name = "Test", Age = 25 }; // Age is different

        // Act & Assert
        Action act = () => TestHelpers.AreTheySame(obj1, obj2);

        act.Should().ThrowExactly<Xunit.Sdk.XunitException>();
    }

    [Fact]
    public void AreTheySame_ObjectsWithCollections_SameValues_NoExceptions()
    {
        // Arrange
        var list1 = new List<string> { "Item1", "Item2" };
        var obj1 = new { Name = "Test", List = list1 };
        var obj2 = new { Name = "Test", List = new List<string> { "Item1", "Item2" } }; // Same content list

        // Act & Assert
        TestHelpers.AreTheySame(obj1, obj2);
    }

    [Fact]
    public void AreTheySame_ObjectsWithCollections_DifferentValues_ThrowsAssertionError()
    {
        // Arrange
        var list1 = new List<string> { "Item1", "Item2" };
        var obj1 = new { Name = "Test", List = list1 };
        var obj2 = new { Name = "Test", List = new List<string> { "Item1", "Item3" } }; // Different list content

        // Act & Assert
        Action act = () => TestHelpers.AreTheySame(obj1, obj2);

        act.Should().ThrowExactly<Xunit.Sdk.XunitException>();
    }

    [Fact]
    public void ShouldBeSameAs_SimpleObjects_SameValues_NoExceptions()
    {
        // Arrange
        var obj1 = new { Name = "Test", Age = 30 };
        var obj2 = new { Name = "Test", Age = 30 };

        // Act & Assert
        obj1.ShouldBeSameAs( obj2);
    }

    [Fact]
    public void ShouldBeSameAs_SimpleObjects_DifferentValues_ThrowsAssertionError()
    {
        // Arrange
        var obj1 = new { Name = "Test", Age = 30 };
        var obj2 = new { Name = "Test", Age = 25 }; // Age is different

        // Act & Assert
        Action act = () => TestHelpers.AreTheySame(obj1, obj2);

        act.Should().ThrowExactly<Xunit.Sdk.XunitException>();
    }
}
