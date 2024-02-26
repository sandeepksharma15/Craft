using Craft.Domain.HashIdentityKey;
using FluentAssertions;

namespace Craft.Domain.Tests.Keys;

public class HashKeyOptionsTests
{
    [Fact]
    public void Alphabet_DefaultValue_ShouldBe_DefaultAlphabet()
    {
        // Arrange
        var options = new HashKeyOptions();

        // Act & Assert
        options.Alphabet.Should().Be(HashidsNet.Hashids.DEFAULT_ALPHABET);
    }

    [Fact]
    public void MinHashLength_DefaultValue_ShouldBe_10()
    {
        // Arrange
        var options = new HashKeyOptions();

        // Act & Assert
        options.MinHashLength.Should().Be(10);
    }

    [Fact]
    public void Salt_DefaultValue_ShouldBe_CraftDomainKeySalt()
    {
        // Arrange
        var options = new HashKeyOptions();

        // Act & Assert
        options.Salt.Should().Be("CraftDomainKeySalt");
    }

    [Fact]
    public void Steps_DefaultValue_ShouldBe_DefaultSeps()
    {
        // Arrange
        var options = new HashKeyOptions();

        // Act & Assert
        options.Steps.Should().Be(HashidsNet.Hashids.DEFAULT_SEPS);
    }

    // Write a test to test all getter and setter methods
    [Fact]
    public void AllProperties_ShouldBeSet()
    {
        // Arrange
        var options = new HashKeyOptions
        {
            Alphabet = "abcdefghijklmnopqrstuvwxyz",
            MinHashLength = 5,
            Salt = "CraftDomainKeySalt",
            Steps = "1234567890"
        };

        // Act & Assert
        options.Alphabet.Should().Be("abcdefghijklmnopqrstuvwxyz");
        options.MinHashLength.Should().Be(5);
        options.Salt.Should().Be("CraftDomainKeySalt");
        options.Steps.Should().Be("1234567890");
    }
}
