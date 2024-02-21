using System.Linq;
using System.Linq.Expressions;
using Craft.QuerySpec.Extensions;
using Craft.QuerySpec.Helpers;
using Craft.TestHelper;
using Craft.TestHelper.Fixtures;
using Craft.TestHelper.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Craft.QuerySpec.Tests.Extensions;

[Collection(nameof(SystemTestCollectionDefinition))]
public class SearchExtensionTests
{
    private readonly TestDbContext dbContext;

    public SearchExtensionTests()
    {
        // Create a new service provider.
        var services = new ServiceCollection()
            .AddDbContext<TestDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDb");
            });

        var serviceProvider = services.BuildServiceProvider();

        // Get The DbContext instance
        dbContext = serviceProvider.GetRequiredService<TestDbContext>();
    }

    [Fact]
    public void Search_WithEmptyCriterias_ShouldReturnSource()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var criterias = new List<SqlLikeSearchInfo<Company>>();

        // Act
        var result = dbContext.Companies.Search(criterias);

        // Assert
        result.Should().BeEquivalentTo(dbContext.Companies);
    }

    [Fact]
    public void Search_WithMatchingCriterias_ShouldReturnFilteredSource()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        Expression<Func<Company, string>> searchItem = x => x.Name;

        List<SqlLikeSearchInfo<Company>> criterias =
        [
            new() { SearchItem = searchItem, SearchString = "%2" }
        ];

        // Act
        var result = dbContext.Companies.Search(criterias).ToList();

        // Assert
        result.Count.Should().Be(1);
        result[0].Name.Should().Be("Company 2");
    }

    [Fact]
    public void Search_WithNonMatchingCriterias_ShouldReturnEmptySource()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        Expression<Func<Company, string>> searchItem = x => x.Name;

        List<SqlLikeSearchInfo<Company>> criterias =
        [
            new() { SearchItem = searchItem, SearchString = "a" }
        ];

        // Act
        var result = dbContext.Companies.Search(criterias);

        // Assert
        result.Should().BeEmpty();
    }
}
