﻿using FluentAssertions;

namespace Craft.Extensions.Tests.Collections;

public class ListTests
{
    [Fact]
    public void IsIn_Should_Return_False_When_Item_Is_Not_In_List()
    {
        // Arrange
        const int item = 4;
        int[] list = [1, 2, 3];

        // Act
        var result = item.IsIn(list);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsIn_Should_Return_False_When_Item_Is_Not_In_List_With_Multiple_Parameters()
    {
        // Arrange
        const string item = "lion";
        string[] list = ["cat", "dog", "elephant"];

        // Act
        var result = item.IsIn(list);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsIn_Should_Return_True_When_Item_Is_In_List()
    {
        // Arrange
        const int item = 2;
        int[] list = [1, 2, 3];

        // Act
        var result = item.IsIn(list);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsIn_Should_Return_True_When_Item_Is_In_List_With_Multiple_Parameters()
    {
        // Arrange
        const string item = "dog";
        string[] list = ["cat", "dog", "elephant"];

        // Act
        var result = item.IsIn(list);

        // Assert
        result.Should().BeTrue();
    }
}
