using System.Text.Json;
using Craft.Domain.Helpers;
using FluentAssertions;

namespace Craft.Domain.Tests.Helpers;

public class PageResponseTests
{
    private readonly JsonSerializerOptions serializerOptions;

    public PageResponseTests()
    {
        serializerOptions = new();
        serializerOptions.Converters.Add(new PageResponseJsonConverter<string>());
    }

    [Fact]
    public void PageResponse_Constructor_SetsProperties()
    {
        // Arrange
        var items = new List<string> { "Item 1", "Item 2" };
        const int totalCount = 100;
        const int currentPage = 2;
        const int pageSize = 10;

        // Act
        var response = new PageResponse<string>(items, totalCount, currentPage, pageSize);

        // Assert
        response.Items.Should().BeEquivalentTo(items);
        response.TotalCount.Should().Be(totalCount);
        response.CurrentPage.Should().Be(currentPage);
        response.PageSize.Should().Be(pageSize);
    }

    [Fact]
    public void PageResponse_Constructor_WithNullItems_ReturnsEmptyCollection()
    {
        // Arrange
        const int totalCount = 100;
        const int currentPage = 2;
        const int pageSize = 10;

        // Act
        var response = new PageResponse<string>(null, totalCount, currentPage, pageSize);

        // Assert
        response.Items.Should().BeEmpty();
        response.TotalCount.Should().Be(totalCount);
        response.CurrentPage.Should().Be(currentPage);
        response.PageSize.Should().Be(pageSize);
    }

    [Fact]
    public void PageResponse_Serialization_Deserialization()
    {
        // Arrange
        var items = new List<string> { "Item 1", "Item 2" };
        const int totalCount = 100;
        const int currentPage = 2;
        const int pageSize = 10;
        var response = new PageResponse<string>(items, totalCount, currentPage, pageSize);

        var serializedData = JsonSerializer.Serialize(response);

        // Act
        var deserializedResponse = JsonSerializer.Deserialize<PageResponse<string>>(serializedData);

        // Assert
        deserializedResponse.Items.Should().BeEquivalentTo(items);
        deserializedResponse.TotalCount.Should().Be(totalCount);
        deserializedResponse.CurrentPage.Should().Be(currentPage);
        deserializedResponse.PageSize.Should().Be(pageSize);
    }

    [Fact]
    public void CanConvert_WithPageResponseType_ReturnsTrue()
    {
        // Arrange
        var converter = new PageResponseJsonConverter<string>();
        var type = typeof(PageResponse<string>);

        // Assert
        converter.CanConvert(type).Should().BeTrue();
    }

    [Fact]
    public void CanConvert_WithOtherType_ReturnsFalse()
    {
        // Arrange
        var converter = new PageResponseJsonConverter<string>();
        var type = typeof(string); // Or any other type

        // Assert
        converter.CanConvert(type).Should().BeFalse();
    }

    [Fact]
    public void Read_ValidJson_DeserializesCorrectly()
    {
        // Arrange
        const string json = @"{ ""Items"": [ ""Item 1"", ""Item 2"" ], ""CurrentPage"": 2, ""PageSize"": 10, ""TotalCount"": 100 }";
        var expectedItems = new List<string> { "Item 1", "Item 2" };

        // Act
        var response = JsonSerializer.Deserialize<PageResponse<string>>(json, serializerOptions);

        // Assert
        response.Items.Should().BeEquivalentTo(expectedItems);
        response.TotalCount.Should().Be(100);
        response.CurrentPage.Should().Be(2);
        response.PageSize.Should().Be(10);
    }

    [Fact]
    public void Write_PageResponse_SerializesCorrectly()
    {
        // Arrange
        var items = new List<string> { "Item 1", "Item 2" };
        var response = new PageResponse<string>(items, 100, 2, 10);

        // Act
        var serializedJson = JsonSerializer.Serialize(response, serializerOptions);

        // Assert
        serializedJson.Should().BeEquivalentTo("{\"Items\":[\"Item 1\",\"Item 2\"],\"CurrentPage\":2,\"PageSize\":10,\"TotalCount\":100}");
    }
}
