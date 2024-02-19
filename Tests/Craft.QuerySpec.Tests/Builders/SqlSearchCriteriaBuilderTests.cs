using Craft.QuerySpec.Builders;
using Craft.QuerySpec.Helpers;
using Craft.TestHelper.Models;
using FluentAssertions;
using System.Text.Json;

namespace Craft.QuerySpec.Tests.Builders;

public class SqlSearchCriteriaBuilderTests
{
    private readonly JsonSerializerOptions serializeOptions;

    public SqlSearchCriteriaBuilderTests()
    {
        serializeOptions = new JsonSerializerOptions();
        serializeOptions.Converters.Add(new SearchBuilderJsonConverter<Company>());
    }

    [Fact]
    public void Constructor_WhenCalled_ShouldInitializeSearchCriteriaList()
    {
        // Arrange & Act
        var builder = new SqlSearchCriteriaBuilder<Company>();

        // Assert
        builder.SearchCriteriaList.Should().NotBeNull();
        builder.SearchCriteriaList.Should().BeEmpty();
    }

    [Fact]
    public void Add_WithSearchInfo_ShouldAddToSearchCriteriaList()
    {
        // Arrange
        var builder = new SqlSearchCriteriaBuilder<Company>();
        var searchInfo = new SqlLikeSearchInfo<Company>(x => x.Name, "searchString");

        // Act
        builder.Add(searchInfo);

        // Assert
        builder.SearchCriteriaList.Should().Contain(searchInfo);
    }

    [Fact]
    public void Add_WithMemberAndSearchTerm_ShouldAddToSearchCriteriaList()
    {
        // Arrange
        var builder = new SqlSearchCriteriaBuilder<Company>();

        // Act
        builder.Add(x => x.Name, "searchString");

        // Assert
        builder.SearchCriteriaList.Should().HaveCount(1);
    }

    [Fact]
    public void Add_WithMemberNameAndSearchTerm_ShouldAddToSearchCriteriaList()
    {
        // Arrange
        var builder = new SqlSearchCriteriaBuilder<Company>();

        // Act
        builder.Add("Name", "searchString");

        // Assert
        builder.SearchCriteriaList.Should().HaveCount(1);
    }

    [Fact]
    public void Clear_WhenCalled_ShouldClearSearchCriteriaList()
    {
        // Arrange
        var builder = new SqlSearchCriteriaBuilder<Company>();
        builder.Add(x => x.Name, "searchString");

        // Act
        builder.Clear();

        // Assert
        builder.SearchCriteriaList.Should().BeEmpty();
    }

    [Fact]
    public void Remove_WithSearchInfo_ShouldRemoveFromSearchCriteriaList()
    {
        // Arrange
        var builder = new SqlSearchCriteriaBuilder<Company>();
        var searchInfo = new SqlLikeSearchInfo<Company>(x => x.Name, "searchString");
        builder.Add(searchInfo);

        // Act
        builder.Remove(searchInfo);

        // Assert
        builder.SearchCriteriaList.Should().NotContain(searchInfo);
    }

    [Fact]
    public void Remove_WithMemberExpression_ShouldRemoveFromSearchCriteriaList()
    {
        // Arrange
        var builder = new SqlSearchCriteriaBuilder<Company>();
        builder.Add(x => x.Name, "searchString");

        // Act
        builder.Remove(x => x.Name);

        // Assert
        builder.SearchCriteriaList.Should().BeEmpty();
    }

    [Fact]
    public void Remove_WithMemberName_ShouldRemoveFromSearchCriteriaList()
    {
        // Arrange
        var builder = new SqlSearchCriteriaBuilder<Company>();
        builder.Add("Name", "searchString");

        // Act
        builder.Remove("Name");

        // Assert
        builder.SearchCriteriaList.Should().BeEmpty();
    }

    [Fact]
    public void CanConvert_ReturnsTrueForSqlSearchCriteriaBuilderType()
    {
        var converter = new SearchBuilderJsonConverter<TestClass>();
        bool canConvert = converter.CanConvert(typeof(SqlSearchCriteriaBuilder<TestClass>));

        canConvert.Should().BeTrue();
    }

    [Fact]
    public void Read_DeserializesValidJsonToSqlSearchCriteriaBuilder()
    {
        // Arrange
        const string json = "[{\"SearchItem\": \"Name\", \"SearchString\": \"John\", \"SearchGroup\": 1}]";

        // Act
        var searchBuilder = JsonSerializer.Deserialize<SqlSearchCriteriaBuilder<Company>>(json, serializeOptions);

        // Assert
        searchBuilder.Count.Should().Be(1);
        searchBuilder.SearchCriteriaList[0].SearchItem.Body.ToString().Should().Contain("x.Name");
        searchBuilder.SearchCriteriaList[0].SearchString.Should().Be("John");
        searchBuilder.SearchCriteriaList[0].SearchGroup.Should().Be(1);
    }

    [Fact]
    public void Read_ThrowsJsonExceptionForInvalidJsonFormat()
    {
        // Arrange
        const string json = "{}"; // Not an array

        // Act and Assert
        Action act = () => JsonSerializer.Deserialize<SqlSearchCriteriaBuilder<Company>>(json, serializeOptions);

        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void Write_SerializesSqlSearchCriteriaBuilderToJsonCorrectly()
    {
        // Arrange
        var searchBuilder = new SqlSearchCriteriaBuilder<Company>();
        searchBuilder.Add(new SqlLikeSearchInfo<Company>(x => x.Name, "Alice", 2));

        // Act
        var json = JsonSerializer.Serialize(searchBuilder, serializeOptions);

        // Assert
        json.Should().Be("[{\"SearchItem\":\"Name\",\"SearchString\":\"Alice\",\"SearchGroup\":2}]");
    }
}
