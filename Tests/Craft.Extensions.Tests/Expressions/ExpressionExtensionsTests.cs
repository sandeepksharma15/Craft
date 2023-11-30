using Craft.Extensions.Expressions;
using FluentAssertions;

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
        MyClass myClass = new() { PropertyName = "Property", AnotherProperty = 10 };
        // Arrange
        const string propertyName = "PropertyName";

        // Act
        var expression = propertyName.CreateMemberExpression<MyClass>();

        // Assert
        expression.Should().NotBeNull();
        expression.Compile().Invoke(myClass).Should().Be("Property");
    }
}
