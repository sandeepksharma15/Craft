﻿using System.Linq.Expressions;
using Craft.Extensions.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace Craft.Extensions.Tests.Expressions;

public class MyClass
{
    public string PropertyName { get; set; }
    public int AnotherProperty { get; set; }
}

public class ExpressionExtensionsTests
{
    [Theory]
    [InlineData("PropertyName")]
    [InlineData("AnotherProperty")]
    public void CreateMemberExpression_ValidProperty_ShouldNotThrowException(string propertyName)
    {
        // Arrange
        // Act
        Action act = () => propertyName.CreateMemberExpression<MyClass>();

        // Assert
        act.Should().NotThrow<ArgumentException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void CreateMemberExpression_NullOrEmptyPropertyName_ShouldThrowArgumentException(string propertyName)
    {
        // Arrange
        // Act
        Action act = () => propertyName.CreateMemberExpression<MyClass>();

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreateMemberExpression_NonexistentProperty_ShouldThrowArgumentException()
    {
        // Arrange
        const string propertyName = "NonexistentProperty";

        // Act
        Action act = () => propertyName.CreateMemberExpression<MyClass>();

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreateMemberExpression_ValidProperty_ReturnsExpression()
    {
        // Arrange
        MyClass myClass = new() { PropertyName = "Property", AnotherProperty = 10 };
        const string propertyName = "PropertyName";

        // Act
        var expression = propertyName.CreateMemberExpression<MyClass>();

        // Assert
        expression.Should().NotBeNull();
        expression.Compile().Invoke(myClass).Should().Be("Property");
    }

    [Fact]
    public void CreateMemberExpression_WithValidProperty_ShouldReturnValidLambdaExpression()
    {
        // Arrange
        MyClass myClass = new() { PropertyName = "Property", AnotherProperty = 10 };
        var type = typeof(MyClass);
        const string propertyName = "PropertyName";

        // Act
        var result = type.CreateMemberExpression(propertyName);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<LambdaExpression>();

        var compiledLambda = result.Compile();
        compiledLambda.DynamicInvoke(myClass).Should().Be("Property");
    }

    [Fact]
    public void CreateMemberExpression_WithInvalidProperty_ShouldThrowArgumentException()
    {
        // Arrange
        var type = typeof(MyClass);
        const string invalidPropertyName = "InvalidProperty";

        // Act
        Action act = () => type.CreateMemberExpression(invalidPropertyName);

        // Assert
        act.Should().Throw<ArgumentException>();
    }
}
