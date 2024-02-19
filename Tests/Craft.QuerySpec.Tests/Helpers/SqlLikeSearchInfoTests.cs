using System.Linq.Expressions;
using System.Text.Json;

using Craft.QuerySpec.Helpers;

using FluentAssertions;

namespace Craft.QuerySpec.Tests.Helpers;

public class SqlLikeSearchInfoTests
{
    private JsonSerializerOptions serializeOptions;

    [Fact]
    public void Constructor_InitializationWithValidValues()
    {
        // Arrange
        Expression<Func<MyResult, string>> searchItem = x => x.ResultName;
        const string searchString = "x%y";
        const int searchGroup = 2;

        // Act
        var searchInfo = new SqlLikeSearchInfo<MyResult>(searchItem, searchString, searchGroup);

        // Assert
        searchInfo.SearchGroup.Should().Be(searchGroup);
        searchInfo.SearchString.Should().Be(searchString);
        searchInfo.SearchItem.Should().Be(searchItem);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_InvalidSearchTerm_ThrowsException(string searchString)
    {
        // Arrange
        Expression<Func<MyResult, string>> searchItem = x => x.ResultName;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new SqlLikeSearchInfo<MyResult>(searchItem, searchString));
    }

    [Fact]
    public void Constructor_NullSearchExpression_ThrowsException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SqlLikeSearchInfo<object>(null, "validTerm"));
    }

    [Fact]
    public void DefaultConstructor_Initialization()
    {
        // Arrange & Act
        var searchInfo = new SqlLikeSearchInfo<MyResult>();

        // Assert
        searchInfo.SearchGroup.Should().Be(0); // Assuming default value is 0.
        searchInfo.SearchString.Should().BeNull();
        searchInfo.SearchItem.Should().BeNull();
    }

    [Fact]
    public void Serialization_RoundTrip_ShouldPreserveSearchItem()
    {
        // Arrange
        Expression<Func<MyResult, string>> searchItem = x => x.ResultName;
        const string searchString = "x%y";
        const int searchGroup = 2;
        var searchInfo = new SqlLikeSearchInfo<MyResult>(searchItem, searchString, searchGroup);

        serializeOptions = new JsonSerializerOptions();
        serializeOptions.Converters.Add(new SqlLikeSearchInfoJsonConverter<MyResult>());

        // Act
        var serializationInfo = JsonSerializer.Serialize(searchInfo, serializeOptions);
        var deserializedInfo = JsonSerializer.Deserialize<SqlLikeSearchInfo<MyResult>>(serializationInfo, serializeOptions);

        // Assert
        deserializedInfo.SearchItem.Should().NotBeNull();
        deserializedInfo.SearchString.Should().Be(searchString);
        deserializedInfo.SearchGroup.Should().Be(searchGroup);
        deserializedInfo.SearchItem.Should().BeEquivalentTo(searchItem);
    }

    private class MyResult
    {
        public long Id { get; set; }
        public string ResultName { get; set; } = default!;
    }
}
