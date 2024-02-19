using System.Text.Json;
using Castle.Core.Resource;
using Craft.QuerySpec.Core;
using Craft.QuerySpec.Helpers;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Core;

public class QueryTests
{
    private readonly IQueryable<Company> queryable;

    public QueryTests()
    {
        queryable = new List<Company>
        {
            new() { Id = 1, Name = "Company 1" },
            new() { Id = 2, Name = "Company 2" }
        }.AsQueryable();
    }

    [Fact]
    public void SetPage_WithValidPageSizeAndPage_ReturnsCorrectSkipAndTake()
    {
        // Arrange
        var query = new Query<Company>();
        const int pageSize = 10;
        const int page = 2;

        // Act
        query.SetPage(page, pageSize);

        // Assert
        query.Take.Should().Be(pageSize);
        query.Skip.Should().Be((page - 1) * pageSize);
    }

    [Theory]
    [InlineData(-5, 10)] // Negative page
    [InlineData(0, 10)] // Zero page
    [InlineData(1, -10)] // Negative pageSize
    [InlineData(1, 0)] // Zero pageSize
    public void SetPage_WithInvalidParameters_SetsToDefaults(int page, int pageSize)
    {
        // Arrange
        var query = new Query<Company>();

        // Act
        query.SetPage(page, pageSize);

        // Assert
        query.Take.Should().Be(10);
        query.Skip.Should().Be(10 * (1 - 1));
    }

    [Fact]
    public void IsSatisfiedBy_ReturnsTrue_ForMatchingEntity()
    {
        // Arrange
        var query = new Query<Company>().Where(e => e.Id == 1);

        // Act
        var result = query.IsSatisfiedBy(queryable.First());

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsSatisfiedBy_ReturnsFalse_ForNonMatchingEntity()
    {
        // Arrange
        var query = new Query<Company>().Where(e => e.Id == 1);

        // Act
        var result = query.IsSatisfiedBy(queryable.Last());

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Clear_ResetsAllQuerySpecifications()
    {
        // Arrange
        var query = new Query<Company, object>
        {
            // Set some query specifications
            AsNoTracking = true,
            Skip = 10
        };
        query.Where(c => c.Name == "John");
        query.Select(c => c.Name);

        // Act
        query.Clear();

        // Assert
        query.AsNoTracking.Should().BeFalse();
        query.Skip.Should().Be(0);
        query.WhereBuilder.EntityFilterList.Count.Should().Be(0);
        query.SelectBuilder.Count.Should().Be(0);
        query.SelectorMany.Should().BeNull();
    }

    [Fact]
    public void SerializeDeserialize_SimpleQuery_PreservesValues()
    {
        // Arrange
        var query = new Query<Company, object>
        {
            // Set some query specifications
            AsNoTracking = true,
            Skip = 10
        };
        query.Where(c => c.Name == "John");
        query.Select(c => c.Name);
        query.OrderBy(c => c.Id);

        // Act
        var serializeOptions = new JsonSerializerOptions();
        serializeOptions.Converters.Add(new OrderInfoJsonConverter<Company>());
        serializeOptions.Converters.Add(new SelectionDescriptorJsonConverter<Company, object>());
        serializeOptions.Converters.Add(new EntityFilterCriteriaJsonConverter<Company>());
        serializeOptions.Converters.Add(new SqlLikeSearchInfoJsonConverter<Company>());

        var serializedQuery = JsonSerializer.Serialize(query, serializeOptions);
        var deserializedQuery = JsonSerializer.Deserialize<Query<Company, object>>(serializedQuery, serializeOptions);

        // Assert
        deserializedQuery.Should().NotBeNull();
        deserializedQuery.WhereBuilder.Count.Should().Be(1);
        deserializedQuery.OrderBuilder.OrderDescriptorList.Count.Should().Be(1);
        deserializedQuery.AsNoTracking.Should().BeTrue();
        deserializedQuery.Skip.Should().Be(10);
        deserializedQuery.SelectBuilder.Count.Should().Be(1);
    }
}
