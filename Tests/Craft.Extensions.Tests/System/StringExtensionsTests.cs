using FluentAssertions;

namespace Craft.Extensions.Tests.System;

public class StringExtensionsTests
{
    [Fact]
    public void RemoveExtraSpaces_NullString_ReturnsNull()
    {
        // Arrange
        const string input = null;

        // Act
        var result = input.RemoveExtraSpaces();

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("", "")]
    [InlineData("  single   space  ", "single space")]
    [InlineData("  multiple   spaces   between   words  ", "multiple spaces between words")]
    [InlineData("   leading   spaces", "leading spaces")]
    [InlineData("trailing   spaces   ", "trailing spaces")]
    [InlineData("   leading   and   trailing   spaces   ", "leading and trailing spaces")]
    public void RemoveExtraSpaces_ValidInput_ReturnsExpectedResult(string input, string expectedResult)
    {
        // Act
        var result = input.RemoveExtraSpaces();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("example.file.txt", '.', "txt")]
    [InlineData("path/to/some/file.txt", '/', "file.txt")]
    [InlineData(null, '.', null)]
    [InlineData("no_delimiter", '.', "no_delimiter")]
    [InlineData("delimiter_at_the_end.", '.', "")]
    public void GetStringAfterLastDelimiter_WithVariousInputs_ReturnsExpectedResult(string input, char delimiter, string expected)
    {
        // Act
        string result = input.GetStringAfterLastDelimiter(delimiter);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("   ", true)]
    [InlineData("abc", false)]
    [InlineData("  abc  ", false)]
    [InlineData("\t", true)]
    public void IsEmpty_ShouldReturnCorrectResult(string input, bool expectedResult)
    {
        // Act
        var result = input.IsEmpty();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("abc", true)]
    [InlineData("  abc  ", true)]
    [InlineData("\t", false)]
    public void IsNonEmpty_ShouldReturnCorrectResult(string input, bool expectedResult)
    {
        // Act
        var result = input.IsNonEmpty();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("   ", false)]
    [InlineData("abc", false)]
    [InlineData("  abc  ", false)]
    public void IsNullOrEmpty_ShouldReturnCorrectResult(string input, bool expectedResult)
    {
        // Act
        var result = input.IsNullOrEmpty();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(null, true)]
    [InlineData("", true)]
    [InlineData("   ", true)]
    [InlineData("abc", false)]
    [InlineData("  abc  ", false)]
    public void IsNullOrWhiteSpace_ShouldReturnCorrectResult(string input, bool expectedResult)
    {
        // Act
        var result = input.IsNullOrWhiteSpace();

        // Assert
        result.Should().Be(expectedResult);
    }
}
