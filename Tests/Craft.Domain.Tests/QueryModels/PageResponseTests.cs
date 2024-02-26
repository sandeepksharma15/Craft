using Craft.Domain.QueryModels;
using FluentAssertions;

namespace Craft.Domain.Tests.QueryModels;

public class PageResponseTests
{
    [Fact]
    public void PageResponse_Constructor_SetsProperties()
    {
        // Arrange
        var items = new List<string> { "Item 1", "Item 2" };
        const int totalCount = 100;
        const int currentPage = 2;
        const int pageSize = 10;

        // Act
        var response = new PageResponse<string>(items, totalCount, currentPage, pageSize);

        // Assert
        response.Items.Should().BeEquivalentTo(items);
        response.TotalCount.Should().Be(totalCount);
        response.CurrentPage.Should().Be(currentPage);
        response.PageSize.Should().Be(pageSize);
    }

    [Fact]
    public void PageResponse_Constructor_WithNullItems_ReturnsEmptyCollection()
    {
        // Arrange
        const int totalCount = 100;
        const int currentPage = 2;
        const int pageSize = 10;

        // Act
        var response = new PageResponse<string>(null, totalCount, currentPage, pageSize);

        // Assert
        response.Items.Should().BeEmpty();
        response.TotalCount.Should().Be(totalCount);
        response.CurrentPage.Should().Be(currentPage);
        response.PageSize.Should().Be(pageSize);
    }
}
