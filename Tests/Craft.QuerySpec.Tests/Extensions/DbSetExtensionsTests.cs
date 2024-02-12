using Craft.QuerySpec.Core;
using Craft.TestHelper.Fixtures;
using Craft.TestHelper.Models;
using FluentAssertions;
using Craft.QuerySpec.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Craft.TestHelper;

namespace AppSpec.Tests.Extensions;

[Collection(nameof(SystemTestCollectionDefinition))]
public class DbSetExtensionsTests
{
    private readonly TestDbContext dbContext;

    public DbSetExtensionsTests()
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
    public async Task ToEnumerableAsync_Returns_Results()
    {
        // Arrange
        dbContext.Database.EnsureCreated();

        var query = new Query<Company>();
        query.Where(u => u.Name == CompanySeed.COMPANY_NAME_1);

        // Act
        var result = (await dbContext.Companies.ToEnumerableAsync(query)).ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Name.Should().Be(CompanySeed.COMPANY_NAME_1);
    }

    [Fact]
    public async Task ToListAsync_Returns_Results()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var query = new Query<Company>();
        query.Where(u => u.Name == CompanySeed.COMPANY_NAME_1);

        // Act
        var result = await dbContext.Companies.ToListAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Name.Should().Be(CompanySeed.COMPANY_NAME_1);
    }

    [Fact]
    public void WithQuery_Returns_IQueryable()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var query = new Query<Company>();
        query.Where(u => u.Name == CompanySeed.COMPANY_NAME_1);

        // Act
        var result = dbContext.Companies.WithQuery(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result.Should().BeAssignableTo<IQueryable<Company>>();
    }

    [Fact]
    public void WithQueryWithTResult_Returns_IQueryable()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var query = new Query<Company, object>();
        query.Where(u => u.Name == CompanySeed.COMPANY_NAME_1);
        query.Select(u => u.Name);

        // Act
        var result = dbContext.Companies.WithQuery(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
    }
}
