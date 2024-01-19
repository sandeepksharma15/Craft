using FluentAssertions;

namespace Craft.Extensions.Tests.Collections;

public class EnumerableExtensionsTests
{
    [Fact]
    public void GetListDataForSelect_Should_Return_Dictionary_With_Correct_Values_When_ValueField_And_DisplayField_Are_Null()
    {
        // Arrange
        var items = new List<string> { "Item 1", "Item 2" };
        const string valueField = null;
        const string displayField = null;

        // Act
        var result = items.GetListDataForSelect(valueField, displayField);

        // Assert
        result.Should().ContainKey("Item 1").And.ContainValue("Item 1");
        result.Should().ContainKey("Item 2").And.ContainValue("Item 2");
        result.Should().HaveCount(2);
    }

    [Fact]
    public void GetListDataForSelect_Should_Return_Dictionary_With_Correct_Values_When_ValueField_And_DisplayField_Are_Provided()
    {
        // Arrange
        var items = new List<ListItem>
        {
            new() { Id = 1, Name = "Item 1" },
            new() { Id = 2, Name = "Item 2" }
        };

        const string valueField = "Id";
        const string displayField = "Name";

        // Act
        var result = items.GetListDataForSelect(valueField, displayField);

        // Assert
        result.Should().ContainKey("1").And.ContainValue("Item 1");
        result.Should().ContainKey("2").And.ContainValue("Item 2");
        result.Should().HaveCount(2);
    }

    [Fact]
    public void GetListDataForSelect_Should_Return_Empty_Dictionary_When_Items_Collection_Is_Empty()
    {
        // Arrange
        var items = new List<ListItem>();
        const string valueField = "Id";
        const string displayField = "Name";

        // Act
        var result = items.GetListDataForSelect(valueField, displayField);

        // Assert
        result.Should().BeEmpty();
    }
}

public class ListItem
{
    public int Id { get; set; }
    public string Name { get; set; }
}
