using Craft.QuerySpec.Helpers;
using Craft.TestHelper.Models;
using FluentAssertions;
using System.Linq.Expressions;
using System.Text.Json;

namespace Craft.QuerySpec.Tests.Helpers;

public class MyResult
{
    public long Id { get; set; }
    public string ResultName { get; set; }
}

public class SelectInfoTests
{
    [Fact]
    public void Constructor_WithValidExpression_ShouldSetSelectItemAndSelectItemFunc()
    {
        // Arrange
        Expression<Func<Company, object>> validExpression = x => x.Name;

        // Act
        var selectInfo = new SelectInfo<Company, Company>(validExpression);

        // Assert
        selectInfo.Assignor.Should().BeEquivalentTo(validExpression);
        selectInfo.Assignor.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithValidPropName_ShouldSetSelectItemAndSelectItemFunc()
    {
        // Arrange
        // Act
        var selectInfo = new SelectInfo<Company, Company>("Name");

        // Assert
        selectInfo.Assignor.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithInvalidPropName_ShouldThrowArgumentNullException()
    {
        // Arrange

        // Act
        Action act = () => new SelectInfo<Company, Company>("InvalidProperty");

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WithNullPropName_ShouldThrowArgumentNullException()
    {
        // Arrange
        const string propName = null;

        // Act
        Action act = () => new SelectInfo<Company, Company>(propName);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WithNullExpression_ShouldThrowArgumentNullException()
    {
        // Arrange
        Expression<Func<Company, object>> nullExpression = null;

        // Act
        Action act = () => new SelectInfo<Company, Company>(nullExpression);

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void SelectInfo_ConstructAssignor_WhenTAndTResultAreSame_ShouldNotThrow()
    {
        // Arrange
        Expression<Func<Company, object>> assignor = e => e.Name;

        // Act & Assert
        new Action(() => new SelectInfo<Company, Company>(assignor)).Should().NotThrow();
    }

    [Fact]
    public void SelectInfo_ConstructAssignor_WhenTAndTResultAreNotSame_ShouldCreateAssignee()
    {
        // Arrange
        Expression<Func<Company, long>> assignor = e => e.Id;

        // Act
        var selectInfo = new SelectInfo<Company, MyResult>(assignor);

        // Assert
        selectInfo.Assignor.Should().Be(assignor);
        selectInfo.Assignee.Should().NotBeNull();
    }

    [Fact]
    public void SelectInfo_ConstructWithAssignorAndAssignee_ShouldNotThrow()
    {
        // Arrange
        Expression<Func<Company, object>> assignor = e => e.Name;
        Expression<Func<MyResult, object>> assignee = re => re.ResultName;

        // Act & Assert
        new Action(() => new SelectInfo<Company, MyResult>(assignor, assignee)).Should().NotThrow();
    }

    [Fact]
    public void SelectInfo_ConstructWithAssignorAndAssigneePropNames_ShouldNotThrow()
    {
        // Arrange

        // Act
        var selectInfo = new SelectInfo<Company, MyResult>("Name", "ResultName");

        // Assert
        selectInfo.Assignor.Should().NotBeNull();
        selectInfo.Assignee.Should().NotBeNull();
    }

    [Fact]
    public void SelectInfo_ConstructWithAssignorAndNullAssignee_ShouldThrowArgumentException()
    {
        // Arrange
        Expression<Func<Company, object>> assignor = e => e.Name;

        // Act & Assert
        new Action(() => new SelectInfo<Company, MyResult>(assignor, null))
            .Should().Throw<ArgumentException>().WithMessage("You must pass a lambda for the assignee");
    }

    [Fact]
    public void Serialization_RoundTrip_ShouldPreserveSelectItem()
    {
        JsonSerializerOptions serializeOptions;

        // Arrange
        Expression<Func<Company, string>> assignorExpression = x => x.Name;
        Expression<Func<MyResult, string>> assigneeExpression = x => x.ResultName;

        var selectInfo = new SelectInfo<Company, MyResult>(assignorExpression, assigneeExpression);

        serializeOptions = new JsonSerializerOptions();
        serializeOptions.Converters.Add(new SelectInfoJsonConverter<Company, MyResult>());

        // Act
        var serializationInfo = JsonSerializer.Serialize(selectInfo, serializeOptions);
        var deserializedSelectInfo = JsonSerializer.Deserialize<SelectInfo<Company, MyResult>>(serializationInfo, serializeOptions);

        // Assert
        deserializedSelectInfo.Assignor.Should().BeEquivalentTo(assignorExpression);
        deserializedSelectInfo.Assignee.Should().BeEquivalentTo(assigneeExpression);
    }
}
