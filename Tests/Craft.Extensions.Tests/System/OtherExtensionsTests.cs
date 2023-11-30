using FluentAssertions;

namespace Craft.Extensions.Tests.System;

public class OtherExtensionsTests
{
    [Theory]
    [InlineData(0, "0%")]
    [InlineData(0.1234, "12.34%")]
    [InlineData(-0.1234, "-12.34%")]
    [InlineData(1, "100%")]
    [InlineData(-1, "-100%")]
    [InlineData(0.005, "0.5%")]
    [InlineData(123.456, "12345.6%")]
    [InlineData(-123.456, "-12345.6%")]
    public void ToPercentage_ShouldConvertDecimalToPercentage(decimal input, string expected)
    {
        // Act
        var result = input.ToPercentage();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0, "0%")]
    [InlineData(0.1234, "12.34%")]
    [InlineData(-0.1234, "-12.34%")]
    [InlineData(1, "100%")]
    [InlineData(-1, "-100%")]
    [InlineData(0.005, "0.5%")]
    [InlineData(123.456, "12345.6%")]
    [InlineData(-123.456, "-12345.6%")]
    public void ToPercentage_ShouldConvertDoubleToPercentage(double input, string expected)
    {
        // Act
        var result = input.ToPercentage();

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(0f, "0%")]
    [InlineData(0.1234f, "12.34%")]
    [InlineData(-0.1234f, "-12.34%")]
    [InlineData(1f, "100%")]
    [InlineData(-1f, "-100%")]
    [InlineData(0.005f, "0.5%")]
    [InlineData(123.456f, "12345.6%")]
    [InlineData(-123.456f, "-12345.6%")]
    public void ToPercentage_ShouldConvertFloatToPercentage(float input, string expected)
    {
        // Act
        var result = input.ToPercentage();

        // Assert
        result.Should().Be(expected);
    }
}
