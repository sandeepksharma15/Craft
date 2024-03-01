using Craft.Core.Repositories;
using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Core;
using Craft.TestHelper;
using Craft.TestHelper.Fixtures;
using Craft.TestHelper.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Craft.Core.Tests.Repositories;

[Collection(nameof(SystemTestCollectionDefinition))]
public class RepositoryTests
{
    private readonly DbContextOptions<TestDbContext> options;
    private readonly IQuery<Country> countryQuery = new Query<Country>();
    private readonly IQuery<Company, CompanyName> companyQuery = new Query<Company, CompanyName>();

    public RepositoryTests()
    {
        options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntities()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureCreated();

            var _logger = new Logger<Repository<Country>>(new LoggerFactory());
            var _repository = new Repository<Country>(context, _logger);

            var countriesToAdd = new List<Country>
            {
                new() { Name = "Country A", IsDeleted = false },
                new() { Name = "Country B", IsDeleted = false }
            };
            context.Countries.AddRange(countriesToAdd);
            await context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(countryQuery);

            // Assert
            context.Countries.Where(c => !c.IsDeleted).Should().BeEmpty();
        }
    }

    [Fact]
    public async Task GetAsync_ShouldReturnSingleEntity()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureCreated();

            var _logger = new Logger<Repository<Country>>(new LoggerFactory());
            var _repository = new Repository<Country>(context, _logger);

            var countriesToAdd = new List<Country>
            {
                new() { Name = "Country A", IsDeleted = false },
                new() { Name = "Country B", IsDeleted = false }
            };
            context.Countries.AddRange(countriesToAdd);
            await context.SaveChangesAsync();

            countryQuery.Where(c => c.Name.StartsWith("Country A"));

            // Act
            var entity = await _repository.GetAsync(countryQuery);

            // Assert
            entity.Should().NotBeNull();
            entity.Name.Should().Be("Country A");
        }
    }

    [Fact]
    public async Task GetResultAsync_ShouldReturnSingleEntity()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureCreated();

            var _logger = new Logger<Repository<Company>>(new LoggerFactory());
            var _repository = new Repository<Company>(context, _logger);

            companyQuery.Where(c => c.Name.Contains('1'));
            companyQuery.Select(c => c.Name);

            // Act
            var entity = await _repository.GetAsync(companyQuery);

            // Assert
            entity.Should().NotBeNull();
            entity.Name.Should().Be(CompanySeed.COMPANY_NAME_1);
        }
    }

    [Fact]
    public async Task GetCountAsync_ShouldReturnEntityCount()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var _logger = new Logger<Repository<Country>>(new LoggerFactory());
            var _repository = new Repository<Country>(context, _logger);

            // Act
            var count = await _repository.GetCountAsync(countryQuery);

            // Assert
            count.Should().Be(2);
        }
    }

    [Fact]
    public async Task GetCountAsync_ShouldReturnZero_WhenConditionResultsInNoEntity()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var _logger = new Logger<Repository<Country>>(new LoggerFactory());
            var _repository = new Repository<Country>(context, _logger);

            // Act
            countryQuery.Clear();
            countryQuery.Where(c => c.Name.StartsWith("Country C"));
            var count = await _repository.GetCountAsync(countryQuery);

            // Assert
            count.Should().Be(0);
        }
    }

    [Fact]
    public async Task GetPagedListAsync_ShouldReturnAll_WhenQueryHasNoConditionIsNull()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var _logger = new Logger<Repository<Country>>(new LoggerFactory());
            var _repository = new Repository<Country>(context, _logger);

            // Act
            countryQuery.Clear();
            countryQuery.SetPage(1, 10);
            var info = await _repository.GetPagedListAsync(countryQuery);

            // Assert
            info.TotalCount.Should().Be(2);
        }
    }

    [Fact]
    public async Task GetPagedListResultAsync_ShouldReturnAll_WhenQueryHasNoConditionIsNull()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var _logger = new Logger<Repository<Company>>(new LoggerFactory());
            var _repository = new Repository<Company>(context, _logger);

            // Act
            companyQuery.Clear();
            companyQuery.SetPage(1, 10);
            companyQuery.Select(c => c.Name);

            var info = await _repository.GetPagedListAsync(companyQuery);

            // Assert
            info.TotalCount.Should().Be(3);
        }
    }

    [Fact]
    public async Task GetPagedListAsync_ShouldReturnPagedItems()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var _logger = new Logger<Repository<Country>>(new LoggerFactory());
            var _repository = new Repository<Country>(context, _logger);

            var countriesToAdd = new List<Country>
            {
                new() { Name = "Country A" },
                new() { Name = "Country B" },
                new() { Name = "Country C" },
                new() { Name = "Country D" }
            };

            context.Countries.AddRange(countriesToAdd);
            await context.SaveChangesAsync();

            countryQuery.Where(c => c.Name.StartsWith("Country"));

            const int page = 2;
            const int pageSize = 2;

            // Act
            countryQuery.Clear();
            countryQuery.SetPage(page, pageSize);

            var result = await _repository.GetPagedListAsync(countryQuery);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(pageSize);
            result.TotalCount.Should().Be(6);
        }
    }
}
