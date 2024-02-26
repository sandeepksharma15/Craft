using Craft.Domain.HashIdentityKey;
using FluentAssertions;

namespace Craft.Domain.Tests.Keys;

public class KeyTypeTests
{
    [Fact]
    public void KeyType_Equal_ShouldReturnTrue_WhenIdIsAssigned()
    {
        // Arrange
        const int id1 = 1;
        const int id2 = id1;

        // Act
        var result = id1.Equals(id2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void KeyType_Equals_ShouldReturnFalse_WhenIdsAreNotEqual()
    {
        // Arrange
        const int id1 = 1;
        const int id2 = 2;

        // Act
        var result = id1.Equals(id2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void KeyType_Equals_ShouldReturnTrue_WhenIdsAreEqual()
    {
        // Arrange
        const int id1 = 1;
        const int id2 = 1;

        // Act
        var result = id1.Equals(id2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void KeyType_ImplicitConversionFromInt_ShouldCreateKeyType()
    {
        // Arrange
        const int value = 1;

        // Act
        const KeyType id = value;

        // Assert
        id.Should().Be(1);
    }

    [Fact]
    public void KeyType_ImplicitConversionFromLong_ShouldCreateKeyType()
    {
        // Arrange
        const long value = 1;

        // Act
        const KeyType id = value;

        // Assert
        id.Should().Be(1);
    }

    [Fact]
    public void KeyType_ImplicitConversionFromString_ShouldCreateKeyType()
    {
        // Arrange
        const string value = "1";

        // Act
        KeyType id = long.Parse(value);

        // Assert
        id.Should().Be(1);
    }

    [Fact]
    public void KeyType_ImplicitConversionToInt_ShouldReturnIdValueAsInt()
    {
        // Arrange
        const int id = 1;

        // Act
        const int value = id;

        // Assert
        value.Should().Be(1);
    }

    [Fact]
    public void KeyType_ImplicitConversionToLong_ShouldReturnIdValue()
    {
        // Arrange
        const int id = 1;

        // Act
        const long value = id;

        // Assert
        value.Should().Be(1);
    }

    [Fact]
    public void KeyType_Parse_ShouldParseStringValueToLong()
    {
        // Arrange
        const string value = "1";

        // Act
        var result = KeyType.Parse(value);

        // Assert
        result.Should().Be(1);
    }

    [Fact]
    public void KeyType_TryParse_WithInvalidStringValue_ShouldReturnFalse()
    {
        // Arrange
        const string value = "invalid";

        // Act
        var success = KeyType.TryParse(value, out var result);

        // Assert
        success.Should().BeFalse();
        result.Should().Be(0);
    }

    [Fact]
    public void KeyType_TryParse_WithValidStringValue_ShouldParseSuccessfully()
    {
        // Arrange
        const string value = "1";

        // Act
        var success = KeyType.TryParse(value, out var result);

        // Assert
        success.Should().BeTrue();
        result.Should().Be(1);
    }

    [Fact]
    public void ModelId_CompareTo_ShouldReturnNegativeValue_WhenIdIsLessThanOtherId()
    {
        // Arrange
        const int id1 = 1;
        const int id2 = 2;

        // Act
        int result = id1.CompareTo(id2);

        // Assert
        result.Should().BeNegative();
    }

    [Fact]
    public void ModelId_CompareTo_ShouldReturnPositiveValue_WhenIdIsGreaterThanOtherId()
    {
        // Arrange
        const int id1 = 2;
        const int id2 = 1;

        // Act
        int result = id1.CompareTo(id2);

        // Assert
        result.Should().BePositive();
    }

    [Fact]
    public void ModelId_CompareTo_ShouldReturnZero_WhenIdIsEqualToOtherId()
    {
        // Arrange
        const int id1 = 1;
        const int id2 = 1;

        // Act
        int result = id1.CompareTo(id2);

        // Assert
        result.Should().Be(0);
    }

    [Fact]
    public void ModelId_GetHashCode_ShouldReturnDifferentHashCode_WhenIdsAreNotEqual()
    {
        // Arrange
        const int id1 = 1;
        const int id2 = 2;

        // Act
        int hashCode1 = id1.GetHashCode();
        int hashCode2 = id2.GetHashCode();

        // Assert
        hashCode1.Should().NotBe(hashCode2);
    }

    [Fact]
    public void ModelId_GetHashCode_ShouldReturnSameHashCode_WhenIdsAreEqual()
    {
        // Arrange
        const int id1 = 1;
        const int id2 = 1;

        // Act
        int hashCode1 = id1.GetHashCode();
        int hashCode2 = id2.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void ModelId_ToString_ShouldReturnStringRepresentationOfId()
    {
        // Arrange
        const int id = 1;

        // Act
        var result = id.ToString();

        // Assert
        result.Should().Be("1");
    }

    [Fact]
    public void KeyType_HashKeys_Roundtrip_ShouldWork()
    {
        // Arrange
        const KeyType keyType = 1;

        // Act
        var result = keyType.ToHashKey();
        var id = result.ToKeyType();

        // Assert
        id.Should().Be(keyType);
    }
}
