using System.Linq.Expressions;
using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Core;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Core;

public class QuerySearchExtensionsTests
{
    [Fact]
    public void Search_WithValidArguments_ShouldAddSearchCriteria()
    {
        // Arrange
        var query = new Query<Company>();
        Expression<Func<Company, object>> member = x => x.Name;
        const string searchTerm = "A%C";
        const int searchGroup = 1;

        // Act
        var result = query.Search(member, searchTerm, searchGroup);

        // Assert
        result.Should().BeSameAs(query);
        query.SqlLikeSearchCriteriaBuilder.Count.Should().Be(1);
        var searchCriteria = query.SqlLikeSearchCriteriaBuilder.SqlLikeSearchCriteriaList[0];
        searchCriteria.SearchItem.Should().Be(member);
        searchCriteria.SearchString.Should().Be(searchTerm);
        searchCriteria.SearchGroup.Should().Be(searchGroup);
    }

    [Fact]
    public void Search_ByMemberName_WithValidArguments_ShouldAddSearchCriteria()
    {
        // Arrange
        var query = new Query<Company>();
        const string member = "Name";
        const string searchTerm = "A%C";
        const int searchGroup = 1;

        // Act
        var result = query.Search(member, searchTerm, searchGroup);

        // Assert
        result.Should().BeSameAs(query);
        query.SqlLikeSearchCriteriaBuilder.Count.Should().Be(1);
        var searchCriteria = query.SqlLikeSearchCriteriaBuilder.SqlLikeSearchCriteriaList[0];
        searchCriteria.SearchItem.Body.ToString().Should().Contain(member);
        searchCriteria.SearchString.Should().Be(searchTerm);
        searchCriteria.SearchGroup.Should().Be(searchGroup);
    }

    [Fact]
    public void Search_WithNullQuery_ShouldReturnNull()
    {
        // Arrange
        IQuery<Company> query = null;
        Expression<Func<Company, object>> member = x => x.Name;
        const string searchTerm = "A%C";
        const int searchGroup = 1;

        // Act
        var result = query.Search(member, searchTerm, searchGroup);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Search_WithNullMember_ShouldReturnQuery()
    {
        // Arrange
        var query = new Query<Company>();
        Expression<Func<Company, object>> member = null;
        const string searchTerm = "test";
        const int searchGroup = 1;

        // Act
        var result = query.Search(member, searchTerm, searchGroup);

        // Assert
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void Search_WithNullSearchTerm_ShouldReturnQuery()
    {
        // Arrange
        // Arrange
        var query = new Query<Company>();
        Expression<Func<Company, object>> member = x => x.Name;
        const string searchTerm = null;
        const int searchGroup = 1;

        // Act
        var result = query.Search(member, searchTerm, searchGroup);

        // Assert
        result.Should().BeSameAs(query);
    }

}
