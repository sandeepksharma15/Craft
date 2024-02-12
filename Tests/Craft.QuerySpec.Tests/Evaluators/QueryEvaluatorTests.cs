using Craft.QuerySpec.Core;
using Craft.QuerySpec.Evaluators;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Evaluators;

public class QueryEvaluatorTests
{
    private readonly IQueryable<TestEntity> _testEntities;

    public QueryEvaluatorTests()
    {
        _testEntities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Test1", IsActive = true },
            new() { Id = 2, Name = "Test2", IsActive = false },
            new() { Id = 3, Name = "Test3", IsActive = true },
            new() { Id = 4, Name = "Test4", IsActive = false },
            new() { Id = 5, Name = "Test5", IsActive = true },
        }.AsQueryable();
    }

    [Fact]
    public void GetQuery_WithNullQuery_ReturnsQueryable()
    {
        // Arrange
        var evaluator = QueryEvaluator.Instance;

        // Act
        var result = evaluator.GetQuery(_testEntities, null);

        // Assert
        result.Should().BeEquivalentTo(_testEntities);
    }

    [Fact]
    public void GetQuery_WithEmptyQuery_ReturnsQueryable()
    {
        // Arrange
        var evaluator = QueryEvaluator.Instance;
        var query = new Query<TestEntity>();

        // Act
        var result = evaluator.GetQuery(_testEntities, query);

        // Assert
        result.Should().BeEquivalentTo(_testEntities);
    }

    [Fact]
    public void GetQuery_WithValidQuery_ReturnsQueryable()
    {
        // Arrange
        var evaluator = QueryEvaluator.Instance;
        var query = new Query<TestEntity>()
            .Where(e => e.IsActive)
            .OrderBy(e => e.Id)
            .Skip(1)
            .Take(2);

        // Act
        var result = evaluator.GetQuery(_testEntities, query).ToList();

        // Assert
        result.Should().BeEquivalentTo(_testEntities.Where(e => e.IsActive).OrderBy(e => e.Id).Skip(1).Take(2).ToList());
    }

    [Fact]
    public void GetQuery_WithValidQueryAndSelect_ReturnsQueryable()
    {
        // Arrange
        var evaluator = QueryEvaluator.Instance;
        var query = new Query<TestEntity, TestDto>();

        query
            .Where(e => e.IsActive)
            .OrderBy(e => e.Id)
            .Skip(1)
            .Take(2);

        query.Select(e => e.Id);
        query.Select(e => e.Name);

        // Act
        var result = evaluator.GetQuery(_testEntities, query).ToList();

        // Assert
        result
            .Should()
            .BeEquivalentTo(_testEntities
                .Where(e => e.IsActive)
                .OrderBy(e => e.Id)
                .Skip(1)
                .Take(2)
                .Select(e => new TestDto { Id = e.Id, Name = e.Name })
                .ToList());
    }

    private class TestEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<TestSubEntity> TestSubEntities { get; set; } = [];
    }

    private class TestSubEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    private class TestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
