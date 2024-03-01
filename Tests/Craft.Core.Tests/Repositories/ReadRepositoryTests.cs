using Craft.Core.Repositories;
using Craft.TestHelper;
using Craft.TestHelper.Fixtures;
using Craft.TestHelper.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Craft.Core.Tests.Repositories;

[Collection(nameof(SystemTestCollectionDefinition))]
public class ReadRepositoryTests
{
    private readonly DbContextOptions<TestDbContext> options;

    public ReadRepositoryTests()
    {
        options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllEntities()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var _logger = new Logger<ReadRepository<Country>>(new LoggerFactory());
            var _repository = new ReadRepository<Country>(context, _logger);

            // Act
            var entities = await _repository.GetAllAsync();

            // Assert
            entities.Should().NotBeNull();
            entities.Count.Should().Be(2);
        }
    }

    [Fact]
    public async Task GetAsync_ShouldReturnEntityById()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Arrange

            var _logger = new Logger<ReadRepository<Country>>(new LoggerFactory());
            var _repository = new ReadRepository<Country>(context, _logger);

            // Act
            const int entityId = CountrySeed.COUNTRY_ID_1;
            var entity = await _repository.GetAsync(entityId);

            // Assert
            entity.Should().NotBeNull();
            entity.Id.Should().Be(entityId);
            entity.Name.Should().Be(CountrySeed.COUNTRY_NAME_1);
            entity.Companies.Should().BeNull();
        }
    }

    [Fact]
    public async Task GetAsync_WithIncludeDetails_ShouldReturn_Details()
    {
        await using (var context = new TestDbContext(options))
        {
            // Arrange
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var _logger = new Logger<ReadRepository<Country>>(new LoggerFactory());
            var _repository = new ReadRepository<Country>(context, _logger);

            // Act
            const long entityId = CountrySeed.COUNTRY_ID_1;
            var entity = await _repository.GetAsync(entityId, true);

            // Assert
            entity.Should().NotBeNull();
            entity.Id.Should().Be(entityId);
            entity.Name.Should().Be(CountrySeed.COUNTRY_NAME_1);
            entity.Companies.Should().NotBeNull();
            entity.Companies.Count.Should().Be(2);
        }
    }

    [Fact]
    public async Task GetCountAsync_ShouldReturnEntityCount()
    {
        await using (var context = new TestDbContext(options))
        {
            // Arrange
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var _logger = new Logger<ReadRepository<Country>>(new LoggerFactory());
            var _repository = new ReadRepository<Country>(context, _logger);

            // Act
            var count = await _repository.GetCountAsync();

            // Assert
            count.Should().Be(2);
        }
    }
}
