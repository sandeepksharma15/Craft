﻿using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;
using Xunit.Sdk;

namespace Craft.Extensions.Tests.Reflection;

public class ReflectionTests
{
    [Fact]
    public void GetPropertyInfo_ShouldReturnMatchingProperty_WhenPropertyExists()
    {
        // Arrange
        var objType = typeof(MyClass);
        const string propertyName = "MyProperty";

        // Act
        var propertyInfo = objType.GetPropertyInfo(propertyName);

        // Assert
        propertyInfo.Should().NotBeNull();
        propertyInfo.Name.Should().Be(propertyName);
    }

    [Fact]
    public void GetPropertyInfo_ShouldThrowArgumentException_WhenPropertyDoesNotExist()
    {
        // Arrange
        var objType = typeof(MyClass);
        const string invalidPropertyName = "InvalidProperty";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => objType.GetPropertyInfo(invalidPropertyName));
    }

    [Fact]
    public void GetPropertyInfo_ValidExpression_ReturnsPropertyInfo()
    {
        // Arrange
        Expression<Func<MyClass, int>> expression = obj => obj.MyProperty;

        // Act
        var propertyInfo = expression.GetPropertyInfo();

        // Assert
        propertyInfo.Should().NotBeNull();
        propertyInfo.Name.Should().Be(nameof(MyClass.MyProperty));
    }

    [Fact]
    public void GetPropertyInfo_InvalidExpression_ThrowsArgumentException()
    {
        // Arrange
        Expression<Func<MyClass, int>> expression = _ => MyClass.MyMethod();

        // Act & Assert
        expression.Invoking(e => e.GetPropertyInfo())
            .Should().Throw<ArgumentException>()
            .WithMessage("Invalid expression. Expected a property access expression.");
    }

    [Fact]
    public void GetPropertyInfo_DirectPropertyAccess_ShouldReturnCorrectPropertyInfo()
    {
        // Arrange
        Expression<Func<MyClass, object>> expression = x => x.MyProperty;

        // Act
        var result = expression.GetPropertyInfo();

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(nameof(MyClass.MyProperty));
    }

    [Fact]
    public void GetPropertyInfo_NestedPropertyAccess_ShouldReturnCorrectPropertyInfo()
    {
        // Arrange
        Expression<Func<MyClass, object>> expression = x => x.MyNestedClass.MyNestedProperty;

        // Act
        var result = expression.GetPropertyInfo();

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(nameof(MyClass.MyNestedClass.MyNestedProperty));
    }

    [Fact]
    public void GetPropertyInfo_InvalidExpression_ShouldThrowArgumentException()
    {
        // Arrange
        Expression<Func<MyClass, object>> expression = x => x.ToString();

        // Act & Assert
        expression.Invoking(e => e.GetPropertyInfo())
                  .Should().Throw<ArgumentException>();
    }

    private class MyClass
    {
        public int MyProperty { get; set; }
        public static int MyMethod() => 42;
        public MyNestedClass MyNestedClass { get; set; }
    }

    public class MyNestedClass
    {
        public string MyNestedProperty { get; set; }
    }
}
