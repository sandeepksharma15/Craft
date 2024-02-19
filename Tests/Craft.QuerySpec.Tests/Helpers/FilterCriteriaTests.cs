using System.Linq.Expressions;
using Craft.QuerySpec.Enums;
using Craft.QuerySpec.Helpers;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Helpers;

public class FilterCriteriaTests
{
    [Fact]
    public void GetExpression_WithFilterInfo_ReturnsValidExpression()
    {
        // Arrange
        var filterInfo = new FilterCriteria(typeof(long).FullName, "Name", "1", ComparisonType.EqualTo);

        // Act
        var expression = filterInfo.GetExpression<Company>();

        // Assert
        expression.Body.NodeType.Should().Be(ExpressionType.Equal);
    }

    [Fact]
    public void GetFilterInfo_WithPropertyExpression_ReturnsCorrectFilterInfo()
    {
        // Arrange
        Expression<Func<string, object>> propertyExpression = s => s.Length;

        // Act
        var filterInfo = FilterCriteria.GetFilterInfo(propertyExpression, 10, ComparisonType.GreaterThan);

        // Assert
        filterInfo.TypeName.Should().Be(typeof(int).FullName);
        filterInfo.Name.Should().Be("Length");
        filterInfo.Value.Should().Be("10");
        filterInfo.Comparison.Should().Be(ComparisonType.GreaterThan);
    }

    [Fact]
    public void GetFilterInfo_WithValidWhereExpression_ReturnsCorrectFilterInfo()
    {
        // Arrange
        Expression<Func<string, bool>> whereExpression = s => s.Length > 10;

        // Act
        var filterInfo = FilterCriteria.GetFilterInfo(whereExpression);

        // Assert
        filterInfo.TypeName.Should().Be(typeof(int).FullName);
        filterInfo.Name.Should().Be("Length");
        filterInfo.Value.Should().Be("10");
        filterInfo.Comparison.Should().Be(ComparisonType.GreaterThan);
    }
}
