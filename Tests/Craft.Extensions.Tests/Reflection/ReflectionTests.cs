using System.Linq.Expressions;
using System.Reflection;
using FluentAssertions;

namespace Craft.Extensions.Tests.Reflection;

public class ReflectionTests
{
    [Fact]
    public void GetMemberByName_NestedProperty_ReturnsCorrectPropertyDescriptor()
    {
        // Arrange
        var type = typeof(MyClass);
        const string propertyName = "MyNestedClass.MyNestedProperty";

        // Act
        var result = type.GetMemberByName(propertyName);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be("MyNestedProperty");
    }

    [Fact]
    public void GetMemberByName_SingleLevelProperty_ReturnsCorrectPropertyDescriptor()
    {
        // Arrange
        var type = typeof(MyClass);
        const string propertyName = "MyProperty";

        // Act
        var result = type.GetMemberByName(propertyName);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(propertyName);
    }

    [Fact]
    public void GetMemberName_ValidPropertyExpression_ReturnsCorrectName()
    {
        // Arrange
        Expression<Func<MyClass, int>> propertyExpression = x => x.MyProperty;

        // Act
        var result = propertyExpression.GetMemberName();

        // Assert
        result.Should().Be("MyProperty");
    }

    [Fact]
    public void GetMemberType_NestedPropertyExpression_ReturnsCorrectType()
    {
        // Arrange
        Expression<Func<MyClass, string>> propertyExpression = x => x.MyNestedClass.MyNestedProperty;

        // Act
        var result = propertyExpression.GetMemberType();

        // Assert
        result.Should().Be(typeof(string));
    }

    [Fact]
    public void GetMemberType_ValidPropertyExpression_ReturnsCorrectType()
    {
        // Arrange
        Expression<Func<MyClass, int>> propertyExpression = x => x.MyProperty;

        // Act
        var result = propertyExpression.GetMemberType();

        // Assert
        result.Should().Be(typeof(int));
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
    public void GetPropertyInfo_InvalidExpression_ShouldThrowArgumentException()
    {
        // Arrange
        Expression<Func<MyClass, object>> expression = x => x.ToString();

        // Act & Assert
        expression.Invoking(e => e.GetPropertyInfo())
                  .Should().Throw<ArgumentException>();
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
    public void GetPropertyInfo_WithMemberExpression_ShouldReturnCorrectPropertyInfo()
    {
        // Arrange
        LambdaExpression expr = (Expression<Func<MyClass, object>>)(x => x.MyProperty);

        // Act
        var propertyInfo = expr.GetPropertyInfo();

        // Assert
        propertyInfo.Should().NotBeNull();
        propertyInfo.Name.Should().Be(nameof(MyClass.MyProperty));
    }

    [Fact]
    public void GetPropertyInfo_WithUnaryExpression_ShouldReturnCorrectPropertyInfo()
    {
        // Arrange
        LambdaExpression lambdaExpression = (Expression<Func<MyClass, object>>)(x => x.MyNestedClass.MyNestedProperty);

        // Act
        var propertyInfo = lambdaExpression.GetPropertyInfo();

        // Assert
        propertyInfo.Should().NotBeNull();
        propertyInfo.Name.Should().Be(nameof(MyNestedClass.MyNestedProperty));
    }

    public class MyNestedClass
    {
        public string MyNestedProperty { get; set; }
    }

    private class MyClass
    {
        public MyNestedClass MyNestedClass { get; set; } = new();
        public int MyProperty { get; set; } = default!;

        public static int MyMethod() => 42;
    }
}
