using System.Linq.Expressions;
using System.Text.Json;
using Craft.QuerySpec.Helpers;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Helpers;

public class SearchInfoTests
{
    private JsonSerializerOptions serializeOptions;

    [Fact]
    public void DefaultConstructor_Initialization()
    {
        // Arrange & Act
        var searchInfo = new SearchInfo<MyResult>();

        // Assert
        searchInfo.SearchGroup.Should().Be(0); // Assuming default value is 0.
        searchInfo.SearchTerm.Should().BeNull();
        searchInfo.SearchItem.Should().BeNull();
    }

    [Fact]
    public void Constructor_InitializationWithValidValues()
    {
        // Arrange
        Expression<Func<MyResult, string>> searchItem = x => x.ResultName;
        const string searchTerm = "x%y";
        const int searchGroup = 2;

        // Act
        var searchInfo = new SearchInfo<MyResult>(searchItem, searchTerm, searchGroup);

        // Assert
        searchInfo.SearchGroup.Should().Be(searchGroup);
        searchInfo.SearchTerm.Should().Be(searchTerm);
        searchInfo.SearchItem.Should().Be(searchItem);
    }

    [Fact]
    public void Constructor_NullSearchExpression_ThrowsException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SearchInfo<object>(null, "validTerm"));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Constructor_InvalidSearchTerm_ThrowsException(string searchTerm)
    {
        // Arrange
        Expression<Func<MyResult, string>> searchItem = x => x.ResultName;

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new SearchInfo<MyResult>(searchItem, searchTerm));
    }

    [Fact]
    public void Serialization_RoundTrip_ShouldPreserveSearchItem()
    {
        // Arrange
        Expression<Func<MyResult, string>> searchItem = x => x.ResultName;
        const string searchTerm = "x%y";
        const int searchGroup = 2;
        var searchInfo = new SearchInfo<MyResult>(searchItem, searchTerm, searchGroup);

        serializeOptions = new JsonSerializerOptions();
        serializeOptions.Converters.Add(new SearchInfoJsonConverter<MyResult>());

        // Act
        var serializationInfo = JsonSerializer.Serialize(searchInfo, serializeOptions);
        var deserializedInfo = JsonSerializer.Deserialize<SearchInfo<MyResult>>(serializationInfo, serializeOptions);

        // Assert
        deserializedInfo.SearchItem.Should().NotBeNull();
        deserializedInfo.SearchTerm.Should().Be(searchTerm);
        deserializedInfo.SearchGroup.Should().Be(searchGroup);
        deserializedInfo.SearchItem.Should().BeEquivalentTo(searchItem);
    }

    private class MyResult
    {
        public long Id { get; set; }
        public string ResultName { get; set; }
    }
}
