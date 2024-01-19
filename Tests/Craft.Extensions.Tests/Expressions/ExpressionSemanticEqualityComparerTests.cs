using Craft.Extensions.Expressions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Craft.Extensions.Tests.Expressions;

public class ExpressionSemanticEqualityComparerTests
{
    [Fact]
    public void EqualExpressions_ShouldReturnTrue()
    {
        // Arrange
        var expression1 = Expression.Equal(Expression.Constant(1), Expression.Constant(1));
        var expression2 = Expression.Equal(Expression.Constant(1), Expression.Constant(1));

        // Act 
        var equalityComparer = new ExpressionSemanticEqualityComparer();
        var result = ExpressionEqualityComparer.Instance.Equals(expression1, expression2)
                || equalityComparer.Equals(expression1, expression2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void NotEqualExpressions_ShouldReturnFalse()
    {
        // Arrange
        var expression1 = Expression.NotEqual(Expression.Constant(1), Expression.Constant(1));
        var expression2 = Expression.Equal(Expression.Constant(1), Expression.Constant(1));

        // Act 
        var equalityComparer = new ExpressionSemanticEqualityComparer();
        var result = ExpressionEqualityComparer.Instance.Equals(expression1, expression2)
                || equalityComparer.Equals(expression1, expression2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void DifferentExpressionTypes_ShouldReturnFalse()
    {
        // Arrange
        var expression1 = Expression.Constant(1);
        var expression2 = Expression.Parameter(typeof(int));

        // Act 
        var equalityComparer = new ExpressionSemanticEqualityComparer();
        var result = ExpressionEqualityComparer.Instance.Equals(expression1, expression2)
                || equalityComparer.Equals(expression1, expression2);

        // Assert
        result.Should().BeFalse();
    }
}
