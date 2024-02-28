using Craft.Utilities.Helpers;
using FluentAssertions;

namespace Craft.Utilities.Tests.Helpers;

public class RandomHelperTests
{
    [Fact]
    public void GenerateRandomizedList_ReturnsRandomizedList()
    {
        // Arrange
        var list = new List<int> { 1, 2, 3, 4, 5 };

        // Act
        var randomizedList = RandomHelper.GenerateRandomizedList(list);

        // Assert
        randomizedList.Should().NotBeSameAs(list);
        randomizedList.Should().BeEquivalentTo(list);
    }

    [Fact]
    public void GenerateRandomizedList_MultipleCalls_DifferentResults()
    {
        // Arrange
        var originalList = new List<string> { "A", "B", "C" };

        // Act
        var resultList1 = RandomHelper.GenerateRandomizedList(originalList);
        var resultList2 = RandomHelper.GenerateRandomizedList(originalList);

        // Assert
        resultList1.Should().NotBeSameAs(resultList2);
        resultList1.Should().BeEquivalentTo(originalList);
        resultList2.Should().BeEquivalentTo(originalList);
    }

    [Fact]
    public void GetRandom_ReturnsRandomNumberInRange()
    {
        // Arrange
        const int minValue = 1;
        const int maxValue = 10;

        // Act
        var randomNumber = RandomHelper.GetRandom(minValue, maxValue);

        // Assert
        randomNumber.Should().BeGreaterThanOrEqualTo(minValue);
        randomNumber.Should().BeLessThan(maxValue);
    }

    [Fact]
    public void GetRandom_ReturnsRandomNumberLessThanMaxValue()
    {
        // Arrange
        const int maxValue = 10;

        // Act
        var randomNumber = RandomHelper.GetRandom(maxValue);

        // Assert
        randomNumber.Should().BeLessThan(maxValue);
    }

    [Fact]
    public void GetRandom_Returns_RandomNumbers()
    {
        // Arrange & Act
        var random1 = RandomHelper.GetRandom();
        var random2 = RandomHelper.GetRandom();
        var random3 = RandomHelper.GetRandom();

        // Assert
        random1.Should().NotBe(random2);
        random1.Should().NotBe(random3);
        random2.Should().NotBe(random3);
    }

    [Fact]
    public void GetRandomOf_EmptyArray_ThrowsArgumentException()
    {
        // Act & Assert
        Action act = () => RandomHelper.GetRandomOf<string>();

        act.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void GetRandomOf_NonEmptyArray_ReturnsRandomElement()
    {
        // Arrange
        var colors = new string[] { "Red", "Green", "Blue" };

        // Act
        var randomColor = RandomHelper.GetRandomOf(colors);

        // Assert
        colors.Should().Contain(randomColor);
    }

    [Fact]
    public void GetRandomOfList_EmptyList_ThrowsArgumentNullException()
    {
        // Act & Assert
        Action act = () => RandomHelper.GetRandomOfList<string>([]);

        act.Should().ThrowExactly<ArgumentException>();
    }

    [Fact]
    public void GetRandomOfList_NonEmptyList_ReturnsRandomElement()
    {
        // Arrange
        var fruits = new List<string> { "Apple", "Banana", "Orange" };

        // Act
        var randomFruit = RandomHelper.GetRandomOfList(fruits);

        // Assert
        fruits.Should().Contain(randomFruit);
    }
}
