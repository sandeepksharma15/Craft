using Craft.Domain.Helpers;
using FluentAssertions;

namespace Craft.Domain.Tests.Helpers;

public class PageInfoTests
{
    [Fact]
    public void PaginationInfo_BasicProperties()
    {
        // Arrange
        var info = new PageInfo { CurrentPage = 2, PageSize = 10, TotalCount = 100 };

        // Assert
        info.CurrentPage.Should().Be(2);
        info.PageSize.Should().Be(10);
        info.TotalCount.Should().Be(100);
    }

    [Fact]
    public void PaginationInfo_From_Calculation()
    {
        // Arrange
        var info = new PageInfo { CurrentPage = 2, PageSize = 10, TotalCount = 100 };

        // Assert
        info.From.Should().Be(11);
    }

    [Fact]
    public void PaginationInfo_To_Calculation_FullPage()
    {
        // Arrange
        var info = new PageInfo { CurrentPage = 2, PageSize = 10, TotalCount = 100 };

        // Assert
        info.To.Should().Be(20);
    }

    [Fact]
    public void PaginationInfo_To_Calculation_PartialPage()
    {
        // Arrange
        var info = new PageInfo { CurrentPage = 9, PageSize = 10, TotalCount = 85 };

        // Assert
        info.To.Should().Be(85);
    }

    [Fact]
    public void PaginationInfo_TotalPages_Calculation()
    {
        // Arrange
        var info = new PageInfo { PageSize = 10, TotalCount = 100 };

        // Assert
        info.TotalPages.Should().Be(10);
    }
}
