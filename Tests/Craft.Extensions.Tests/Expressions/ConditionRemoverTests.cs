using Craft.Extensions.Expressions;
using FluentAssertions;
using System.Linq.Expressions;

namespace Craft.Extensions.Tests.Expressions;

public class ConditionRemoverTests
{
    [Fact]
    public void RemoveCondition_WithDifferentConditions_ShouldReturnOriginalExpression()
    {
        // Arrange
        Expression<Func<int, bool>> original = x => x > 5;
        Expression<Func<int, bool>> conditionToRemove = x => x < 10;

        // Act
        var result = original.RemoveCondition(conditionToRemove);

        // Assert
        result.Should().BeEquivalentTo(original);
    }

    [Fact]
    public void RemoveCondition_WithEquivalentConditions_ShouldReturnNull()
    {
        // Arrange
        Expression<Func<int, bool>> original = x => x > 5;
        Expression<Func<int, bool>> equivalentCondition = x => x > 5;

        // Act
        var result = original.RemoveCondition(equivalentCondition);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void RemoveCondition_WithNullOriginalExpression_ShouldThrowArgumentNullException()
    {
        // Arrange
        Expression<Func<int, bool>> original = null;
        Expression<Func<int, bool>> conditionToRemove = x => x < 10;

        // Act & Assert
        Action act = () => original.RemoveCondition(conditionToRemove);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void RemoveCondition_WithNullConditionToRemove_ShouldThrowArgumentNullException()
    {
        // Arrange
        Expression<Func<int, bool>> original = x => x > 5;
        Expression<Func<int, bool>> conditionToRemove = null;

        // Act & Assert
        Action act = () => original.RemoveCondition(conditionToRemove);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void RemoveCondition_WithRemoveCondition_ShouldReturnReducedExpression()
    {
        // Arrange
        Expression<Func<int, bool>> original = x => x > 5 && x < 10;
        Expression<Func<int, bool>> conditionToRemove = x => x < 10;
        Expression<Func<int, bool>> expected = x => x > 5;

        // Act
        var result = original.RemoveCondition(conditionToRemove);

        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}
