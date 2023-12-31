﻿using FluentAssertions;

namespace Craft.Extensions.Tests.System;

public class ObjectTests
{
    [Fact]
    public void AsType_Should_Return_Correct_Type()
    {
        // Arrange
        object obj = "Hello, World!";

        // Act
        var result = obj.AsType<string>();

        // Assert
        result.Should().BeOfType<string>();
        result.Should().Be("Hello, World!");
    }

    [Fact]
    public void AsType_Should_Return_Null_For_Incorrect_Type()
    {
        // Arrange
        object obj = 123;

        // Act
        var action = new Action(() => obj.AsType<string>());

        // Assert
        action.Should().Throw<InvalidCastException>();
    }

    [Fact]
    public void If_Should_Apply_Function_When_Condition_Is_True()
    {
        // Arrange
        const int value = 5;
        const bool condition = true;

        // Act
        var result = value.If(condition, x => x * 2);

        // Assert
        result.Should().Be(10);
    }

    [Fact]
    public void If_Should_Not_Apply_Function_When_Condition_Is_False()
    {
        // Arrange
        const int value = 5;
        const bool condition = false;

        // Act
        var result = value.If(condition, x => x * 2);

        // Assert
        result.Should().Be(5);
    }

    [Fact]
    public void If_Should_Not_Perform_Action_When_Action_Is_Null()
    {
        // Arrange
        const int value = 5;
        const bool condition = true;

        // Act
        int newValue = value.If(condition, null);

        // Assert
        newValue.Should().Be(value);
    }

    [Fact]
    public void If_Should_Not_Perform_Action_When_Condition_Is_False()
    {
        // Arrange
        const int value = 5;
        const bool condition = false;
        int newValue = 0;

        // Act
        value.If(condition, x => newValue = x * 2);

        // Assert
        newValue.Should().Be(0);
    }

    [Fact]
    public void If_Should_Perform_Action_When_Condition_Is_True()
    {
        // Arrange
        const int value = 5;
        const bool condition = true;
        int newValue = 0;

        // Act
        value.If(condition, x => newValue = x * 2);

        // Assert
        newValue.Should().Be(10);
    }

    [Fact]
    public void If_Should_Return_Same_Value_When_Condition_Is_False_And_No_Action_Provided()
    {
        // Arrange
        const string text = "Hello";
        const bool condition = false;

        // Act
        var result = text.If(condition, null);

        // Assert
        result.Should().Be(text);
    }

    [Fact]
    public void If_Should_Return_Same_Value_When_Condition_Is_True_And_No_Function_Provided()
    {
        // Arrange
        const string text = "Hello";
        const bool condition = true;

        // Act
        var result = text.If(condition, null);

        // Assert
        result.Should().Be(text);
    }

    [Fact]
    public void ToValue_Should_Convert_Guid_Correctly()
    {
        // Arrange
        object obj = "6F9619FF-8B86-D011-B42D-00C04FC964FF";

        // Act
        var result = obj.ToValue<Guid>();

        // Assert
        result.Should().Be(Guid.Parse("6F9619FF-8B86-D011-B42D-00C04FC964FF"));
    }

    [Fact]
    public void ToValue_Should_Convert_To_Correct_Type()
    {
        // Arrange
        object obj = "123";

        // Act
        var result = obj.ToValue<int>();

        // Assert
        result.Should().Be(123);
    }

    [Fact]
    public void ToValue_Should_Throw_Exception_For_Invalid_Conversion()
    {
        // Arrange
        object obj = "InvalidNumber";

        // Act
        var action = new Action(() => obj.ToValue<int>());

        // Assert
        action.Should().Throw<FormatException>();
    }
}
