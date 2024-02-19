using System.Linq.Expressions;
using System.Text.Json;
using Craft.QuerySpec.Helpers;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Helpers;

public class EntityFilterCriteriaTests
{
    private readonly JsonSerializerOptions options;

    public EntityFilterCriteriaTests()
    {
        options = new JsonSerializerOptions();
        options.Converters.Add(new EntityFilterCriteriaJsonConverter<MyEntity>());
    }

    [Fact]
    public void Constructor_WithValidFilter_InitializesProperties()
    {
        // Arrange
        Expression<Func<string, bool>> filterExpression = s => s.Length > 5;

        // Act
        var whereInfo = new EntityFilterCriteria<string>(filterExpression);

        // Assert
        whereInfo.Filter.Should().BeEquivalentTo(filterExpression);
        whereInfo.FilterFunc.Invoke("TestString").Should().Be(true);
    }

    [Fact]
    public void Serialization_RoundTrip_ReturnsEqualWhereInfo()
    {
        // Arrange
        Expression<Func<MyEntity, bool>> filterExpression = x => x.Name == "John";
        var whereInfo = new EntityFilterCriteria<MyEntity>(filterExpression);
        var entity = new MyEntity { Name = "John" };

        // Act
        var serializationInfo = JsonSerializer.Serialize(whereInfo, options);
        var deserializedWhereInfo = JsonSerializer.Deserialize<EntityFilterCriteria<MyEntity>>(serializationInfo, options);

        // Assert
        deserializedWhereInfo.Should().NotBeNull();
        deserializedWhereInfo.Filter.Should().BeEquivalentTo(filterExpression);
        deserializedWhereInfo.FilterFunc(entity).Should().BeTrue();
    }

    [Fact]
    public void WhereInfo_Constructor_NullFilter_ThrowsArgumentNullException()
    {
        // Arrange
        Action act = () => new EntityFilterCriteria<MyEntity>(null);

        // Act & Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WhereInfo_Constructor_ValidFilter_CompilesCorrectly()
    {
        // Arrange
        Expression<Func<MyEntity, bool>> filter = x => x.Name == "John";
        var whereInfo = new EntityFilterCriteria<MyEntity>(filter);
        var entity = new MyEntity { Name = "John" };

        // Act & Assert
        whereInfo.FilterFunc(entity).Should().BeTrue();
    }

    [Fact]
    public void WhereInfo_Matches_MatchingEntity_ReturnsTrue()
    {
        // Arrange
        Expression<Func<MyEntity, bool>> filter = x => x.Name == "John";
        var whereInfo = new EntityFilterCriteria<MyEntity>(filter);
        var entity = new MyEntity { Name = "John" };

        // Act & Assert
        whereInfo.Matches(entity).Should().BeTrue();
    }

    [Fact]
    public void WhereInfo_Matches_NonMatchingEntity_ReturnsFalse()
    {
        // Arrange
        Expression<Func<MyEntity, bool>> filter = x => x.Name == "John";
        var whereInfo = new EntityFilterCriteria<MyEntity>(filter);
        var entity = new MyEntity { Name = "Jane" };

        // Act & Assert
        whereInfo.Matches(entity).Should().BeFalse();
    }

    [Fact]
    public void WhereInfoJsonConverter_Read_InvalidJson_ThrowsJsonException()
    {
        // Arrange
        const string json = "{\"Filter\": {"; // Missing closing brace

        // Act & Assert
        Action act = () => JsonSerializer.Deserialize<EntityFilterCriteria<MyEntity>>(json, options);
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void WhereInfoJsonConverter_Read_InvalidProperty_ThrowsJsonException()
    {
        // Arrange
        const string json = "{\"InvalidProperty\": \"Name == 'John'\"}";

        // Act & Assert
        Action act = () => JsonSerializer.Deserialize<EntityFilterCriteria<MyEntity>>(json, options);
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void WhereInfoJsonConverter_Read_InvalidSyntax_ThrowsJsonException()
    {
        // Arrange
        const string json = "{\"Filter\": \"InvalidSyntax\""; // Missing expression body

        // Act & Assert
        Action act = () => JsonSerializer.Deserialize<EntityFilterCriteria<MyEntity>>(json, options);
        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void WhereInfoJsonConverter_Read_NullJson_ThrowsException()
    {
        // Act
        Action act = () => JsonSerializer.Deserialize<EntityFilterCriteria<MyEntity>>("\"null\"", options);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void WhereInfoJsonConverter_Read_ValidJson_ConstructsCorrectly()
    {
        // Arrange
        Expression<Func<MyEntity, bool>> filterExpression = x => x.Name == "John";
        var filter = filterExpression.ToString();
        var serializeFilter = JsonSerializer.Serialize<string>(filter);

        const string json = "{\"Filter\": \"Name == 'John'\"}";

        // Act
        var result = JsonSerializer.Deserialize<EntityFilterCriteria<MyEntity>>(json, options);

        // Assert
        result.Should().NotBeNull();
        result.Filter.Should().BeEquivalentTo(filterExpression);
    }

    private class MyEntity
    {
        public string Name { get; set; }
    }
}
