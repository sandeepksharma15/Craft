using System.Linq.Expressions;
using Craft.TestHelper.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Craft.Extensions.Tests.EfCore;

public class DbSetExtensionsTests
{
    [Fact]
    public void GetQueryFilter_Should_Return_Null_When_No_QueryFilter_Exists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MyAnotherDbContext>()
            .UseInMemoryDatabase(databaseName: "AnotherTestDb")
            .Options;

        using (var context = new MyAnotherDbContext(options))
        {
            // Act
            var result = context.Entities.GetQueryFilter();

            // Assert
            result.Should().BeNull();
        }
    }

    [Fact]
    public void GetQueryFilter_Should_Return_QueryFilter_When_Exists()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        Expression<Func<Entity, bool>> expectedExpression = e => e.IsActive;

        using (var context = new MyDbContext(options))
        {
            // Act
            var result = context.Entities.GetQueryFilter();

            // Assert
            result.Should().NotBeNull();
            result.Parameters.Should().HaveCount(1);
            result.Parameters[0].Name.Should().Be("e");
            result.Body.Should().BeEquivalentTo(expectedExpression.Body);
        }
    }

    [Fact]
    public void RemoveFromQueryFilter_Should_Remove_Condition_From_QueryFilter()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        using (var context = new MyDbContext(options))
        {
            // Apply a query filter to the entity
            context.Entities.Add(new Entity { Id = 1, IsActive = true });
            context.Entities.Add(new Entity { Id = 2, IsActive = false });
            context.SaveChanges();

            // Act
            var result = context.Entities.RemoveFromQueryFilter(e => e.IsActive);

            // Assert
            result.Count().Should().Be(2); // Both entities should be returned

            // Cleanup
            var allEntities = context.Entities.IgnoreQueryFilters().ToList();
            context.Entities.RemoveRange(allEntities);
            context.SaveChanges();
        }
    }

    [Fact]
    public void RemoveFromQueryFilter_Should_Return_All_Entities_When_Condition_Removed_Completely()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        using (var context = new MyDbContext(options))
        {
            // Apply a query filter to the entity
            context.Entities.Add(new Entity { Id = 1, IsActive = true });
            context.Entities.Add(new Entity { Id = 2, IsActive = false });
            context.SaveChanges();

            // Act
#pragma warning disable RCS1033 // Remove redundant boolean literal.
            var result = context.Entities.RemoveFromQueryFilter(e => e.IsActive == true);
#pragma warning restore RCS1033 // Remove redundant boolean literal.

            // Assert
            result.Count().Should().Be(2); // Both entities should be returned

            // Cleanup
            var allEntities = context.Entities.IgnoreQueryFilters().ToList();
            context.Entities.RemoveRange(allEntities);
            context.SaveChanges();
        }
    }

    [Fact]
    public void RemoveFromQueryFilter_Should_Return_Entities_Fulfilling_TheRestFilters()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MyYetAnotherDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        using (var context = new MyYetAnotherDbContext(options))
        {
            // Apply a query filter to the entity
            context.Entities.Add(new Entity { Id = 1, IsActive = true, IsDeleted = false });
            context.Entities.Add(new Entity { Id = 2, IsActive = false, IsDeleted = false });
            context.Entities.Add(new Entity { Id = 3, IsActive = true, IsDeleted = true });
            context.SaveChanges();

            // Act
#pragma warning disable RCS1033 // Remove redundant boolean literal.
            var result = context.Entities.RemoveFromQueryFilter(e => e.IsActive == true);
#pragma warning restore RCS1033 // Remove redundant boolean literal.

            // Assert
            result.Count().Should().Be(2); // Both entities should be returned

            // Cleanup
            var allEntities = context.Entities.IgnoreQueryFilters().ToList();
            context.Entities.RemoveRange(allEntities);
            context.SaveChanges();
        }
    }
}

public class MyDbContext(DbContextOptions<MyDbContext> options) : DbContext(options)
{
    public DbSet<Entity> Entities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure query filter on the entity
        modelBuilder.Entity<Entity>().HasQueryFilter(e => e.IsActive);

        base.OnModelCreating(modelBuilder);
    }
}

public class MyAnotherDbContext(DbContextOptions<MyAnotherDbContext> options) : DbContext(options)
{
    public DbSet<Entity> Entities { get; set; }
}

public class MyYetAnotherDbContext(DbContextOptions<MyYetAnotherDbContext> options) : DbContext(options)
{
    public DbSet<Entity> Entities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure query filter on the entity
        modelBuilder.Entity<Entity>().HasQueryFilter(e => e.IsActive);
        modelBuilder.Entity<Entity>().HasQueryFilter(e => !e.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }
}
