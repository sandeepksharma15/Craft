﻿using FluentAssertions;

namespace Craft.Extensions.Tests.System;

public class OtherExtensionsTests
{
    [Theory]
    [InlineData(null, null)] // Null input should return null
    [InlineData(new byte[0], "")] // Empty byte array should return an empty string
    [InlineData(new byte[] { 0x01, 0xAB, 0xFF }, "01ABFF")] // Typical byte values
    [InlineData(new byte[] { 0x00, 0x0F, 0xFF }, "000FFF")] // Byte values with leading zeros
    public void BytesToHex_ReturnsExpectedResult(byte[] inputBytes, string expectedResult)
    {
        // Act
        var result = inputBytes.BytesToHex();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData("InvalidHex")]
    [InlineData("12345")]
    [InlineData("ABCDEF012G")]
    public void HexToBytes_InvalidInput_ThrowsFormatException(string input)
    {
        // Act & Assert
        input.Invoking(i => i.HexToBytes()).Should().Throw<FormatException>();
    }

    [Theory]
    [InlineData("48656C6C6F", new byte[] { 72, 101, 108, 108, 111 })]
    [InlineData("010203", new byte[] { 1, 2, 3 })]
    [InlineData("", new byte[0])]
    [InlineData(null, new byte[0])]
    public void HexToBytes_ValidInput_ReturnsExpectedByteArray(string input, byte[] expected)
    {
        // Act
        var result = input.HexToBytes();

        // Assert
        result.Should().BeEquivalentTo(expected);
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
