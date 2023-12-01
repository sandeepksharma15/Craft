using FluentAssertions;

namespace Craft.Extensions.Tests.Linq;

public class LinqTests
{
    [Fact]
    public void OrderBy_ShouldSortByPropertyName_Ascending()
    {
        // Arrange
        var data = new[]
        {
            new Person { Id = 3, Name = "John", Age = 25 },
            new Person { Id = 1, Name = "Alice", Age = 30 },
            new Person { Id = 2, Name = "Bob", Age = 20 }
        };

        var queryable = data.AsQueryable();

        // Act
        var result = queryable.OrderBy("Name", false).ToArray();

        // Assert
        result[0].Name.Should().Be("Alice");
        result[1].Name.Should().Be("Bob");
        result[2].Name.Should().Be("John");
    }

    [Fact]
    public void OrderBy_ShouldSortByPropertyName_Descending()
    {
        // Arrange
        var data = new[]
        {
            new Person { Id = 3, Name = "John", Age = 25 },
            new Person { Id = 1, Name = "Alice", Age = 30 },
            new Person { Id = 2, Name = "Bob", Age = 20 }
        };

        var queryable = data.AsQueryable();

        // Act
        var result = queryable.OrderBy("Age", true).ToArray();

        // Assert
        result[0].Age.Should().Be(30);
        result[1].Age.Should().Be(25);
        result[2].Age.Should().Be(20);
    }

    [Fact]
    public void OrderBy_ShouldThrowException_WhenInvalidPropertyName()
    {
        // Arrange
        var data = new[]
        {
            new Person { Id = 1, Name = "Alice", Age = 30 }
        };

        var queryable = data.AsQueryable();

        // Act
        var action = new Action(() => _ = queryable.OrderBy("InvalidProperty", false).ToArray());

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void OrderBy_SortsByMultipleProperties()
    {
        // Arrange
        var data = new List<Person>
        {
            new() { Id = 1, Name = "Charlie", Age = 20 },
            new() { Id = 2, Name = "Bob", Age = 30 },
            new() { Id = 3, Name = "Alice", Age = 25 },
            new() { Id = 4, Name = "Bob", Age = 20 },
        }.AsQueryable();

        // Act
        var result = data.OrderBy("Name ASC, Age DESC");

        // Assert
        Assert.Equal("Alice", result.First().Name);
        Assert.Equal(25, result.First().Age);
        Assert.Equal("Bob", result.Skip(1).First().Name);
        Assert.Equal(30, result.Skip(1).First().Age);
        Assert.Equal("Bob", result.Skip(2).First().Name);
        Assert.Equal(20, result.Skip(2).First().Age);
        Assert.Equal("Charlie", result.Skip(3).First().Name);
        Assert.Equal(20, result.Skip(3).First().Age);
    }

    [Fact]
    public void OrderBy_SortsBySinglePropertyAscending()
    {
        // Arrange
        var data = new List<Person>
        {
            new() { Id = 1, Name = "Charlie", Age = 20 },
            new() { Id = 2, Name = "Bob", Age = 30 },
            new() { Id = 3, Name = "Alice", Age = 25 },
        }.AsQueryable();

        // Act
        var result = data.OrderBy("Name");

        // Assert
        Assert.Equal("Alice", result.First().Name);
        Assert.Equal("Bob", result.Skip(1).First().Name);
        Assert.Equal("Charlie", result.Skip(2).First().Name);
    }

    [Fact]
    public void OrderBy_SortsBySinglePropertyDescending()
    {
        // Arrange
        var data = new List<Person>
        {
            new() { Id = 1, Name = "Charlie", Age = 20 },
            new() { Id = 2, Name = "Bob", Age = 30 },
            new() { Id = 3, Name = "Alice", Age = 25 },
        }.AsQueryable();

        // Act
        var result = data.OrderBy("Name DESC");

        // Assert
        Assert.Equal("Charlie", result.First().Name);
        Assert.Equal("Bob", result.Skip(1).First().Name);
        Assert.Equal("Alice", result.Skip(2).First().Name);
    }

    private class Person
    {
        public int Age { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }
}
