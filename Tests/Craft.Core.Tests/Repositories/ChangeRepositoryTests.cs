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
public class ChangeRepositoryTests
{
    private readonly DbContextOptions<TestDbContext> options;

    public ChangeRepositoryTests()
    {
        options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntity()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var _logger = new Logger<ChangeRepository<Country>>(new LoggerFactory());
            var _repository = new ChangeRepository<Country>(context, _logger);

            // Act
            var countryToAdd = new Country { Name = "Test Country" };
            var addedCountry = await _repository.AddAsync(countryToAdd);

            // Assert
            addedCountry.Should().NotBeNull();
            addedCountry.Id.Should().NotBe(0);
            context.Countries.Should().Contain(c => c.Id == addedCountry.Id);
        }
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddEntities()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var _logger = new Logger<ChangeRepository<Country>>(new LoggerFactory());
            var _repository = new ChangeRepository<Country>(context, _logger);

            var countriesToAdd = new List<Country>
            {
                new() { Name = "Country 3" },
                new() { Name = "Country 4" }
            };

            // Act
            await _repository.AddRangeAsync(countriesToAdd);

            // Assert
            context.Countries.Should().HaveCount(4);
            context.Countries.Should().Contain(countriesToAdd);
        }
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteEntity()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var _logger = new Logger<ChangeRepository<Country>>(new LoggerFactory());
            var _repository = new ChangeRepository<Country>(context, _logger);

            // Act
            var countryToAdd = new Country { Name = "Test Country" };
            var addedCountry = await _repository.AddAsync(countryToAdd);
            context.Countries.Should().Contain(c => c.Id == addedCountry.Id);

            context.ChangeTracker.Clear();

            var countryToDelete = await _repository.GetAsync(addedCountry.Id);
            await _repository.DeleteAsync(countryToDelete);

            // Assert
            var country = await _repository.GetAsync(addedCountry.Id);
            country.IsDeleted.Should().BeTrue();
        }
    }

    [Fact]
    public async Task DeleteRangeAsync_ShouldDeleteEntities()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var _logger = new Logger<ChangeRepository<Country>>(new LoggerFactory());
            var _repository = new ChangeRepository<Country>(context, _logger);

            var countriesToDelete = context.Countries.ToList();

            // Act
            await _repository.DeleteRangeAsync(countriesToDelete);
            var countries = await _repository.GetAllAsync();

            // Assert
            context.Countries.Where(c => !c.IsDeleted).Should().BeEmpty();
        }
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntity()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var _logger = new Logger<ChangeRepository<Country>>(new LoggerFactory());
            var _repository = new ChangeRepository<Country>(context, _logger);

            // Act
            var countryToUpdate = await _repository.GetAsync(CountrySeed.COUNTRY_ID_1);
            countryToUpdate.Name = "Updated Country";
            var updatedCountry = await _repository.UpdateAsync(countryToUpdate);

            // Assert
            updatedCountry.Should().NotBeNull();
            context.Countries.Should().Contain(c => c.Name == "Updated Country");
        }
    }

    [Fact]
    public async Task UpdateRangeAsync_ShouldUpdateEntities()
    {
        await using (var context = new TestDbContext(options))
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var _logger = new Logger<ChangeRepository<Country>>(new LoggerFactory());
            var _repository = new ChangeRepository<Country>(context, _logger);

            var countriesToAdd = new List<Country>
        {
            new() { Name = "Country 3" },
            new() { Name = "Country 4" }
        };
            context.Countries.AddRange(countriesToAdd);
            await context.SaveChangesAsync();

            var countriesToUpdate = context.Countries.ToList();
            foreach (var country in countriesToUpdate)
                country.Name += " Updated";

            // Act
            await _repository.UpdateRangeAsync(countriesToUpdate);

            // Assert
            var updatedCountries = context.Countries.ToList();
            updatedCountries.Should().OnlyContain(c => c.Name.EndsWith("Updated"));
        }
    }
}
