using Craft.Utilities.Builders;
using FluentAssertions;

namespace Craft.Utilities.Tests.Builders;

public class CssBuilderTests
{
    [Fact]
    public void ShouldConstructWithDefaultValue()
    {
        // Arrange
        var classToRender = CssBuilder.Default("item-one").Build();

        // Assert
        classToRender.Should().Be("item-one");
    }

    [Fact]
    public void Build_ShouldReturnTrimmedString()
    {
        // Arrange
        var cssBuilder = new CssBuilder("  test  ");

        // Act
        var result = cssBuilder.Build();

        // Assert
        result.Should().Be("test");
    }

    [Fact]
    public void ToString_ShouldReturnSameResultAsBuild()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");

        // Act
        var buildResult = cssBuilder.Build();
        var toStringResult = cssBuilder.ToString();

        // Assert
        toStringResult.Should().Be(buildResult);
    }

    [Fact]
    public void AddValue_ShouldAppendToBuffer()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");

        // Act
        var result = cssBuilder.AddValue(" appended").Build();

        // Assert
        result.Should().Be("value appended");
    }

    [Fact]
    public void AddClass_ShouldAddSpaceAndClass()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");

        // Act
        var result = cssBuilder.AddClass("newClass").Build();

        // Assert
        result.Should().Be("value newClass");
    }

    [Fact]
    public void AddClass_WithCondition_ShouldAddClassIfConditionIsTrue()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");

        // Act
        var result = cssBuilder.AddClass("newClass", true).Build();

        // Assert
        result.Should().Be("value newClass");
    }

    [Fact]
    public void AddClass_WithCondition_ShouldNotAddClassIfConditionIsFalse()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");

        // Act
        var result = cssBuilder.AddClass("newClass", false).Build();

        // Assert
        result.Should().Be("value");
    }

    [Fact]
    public void AddClass_WithFuncCondition_ShouldAddClassIfFuncConditionIsTrue()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");

        // Act
        var result = cssBuilder.AddClass("newClass", () => true).Build();

        // Assert
        result.Should().Be("value newClass");
    }

    [Fact]
    public void AddClass_WithFuncCondition_ShouldNotAddClassIfFuncConditionIsFalse()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");

        // Act
        var result = cssBuilder.AddClass("newClass", () => false).Build();

        // Assert
        result.Should().Be("value");
    }

    [Fact]
    public void AddClass_WithFuncCondition_ShouldNotAddClassIfFuncConditionIsNull()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");

        // Act
        var result = cssBuilder.AddClass("newClass", (Func<bool>?)null).Build();

        // Assert
        result.Should().Be("value");
    }

    [Fact]
    public void AddClassFromAttributes_ShouldAddClassFromAttributes()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");
        var attributes = new Dictionary<string, object> { { "class", "additionalClass" } };

        // Act
        var result = cssBuilder.AddClassFromAttributes(attributes).Build();

        // Assert
        result.Should().Be("value additionalClass");
    }

    [Fact]
    public void AddClassFromAttributes_WithNullAttributes_ShouldNotAddClass()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");

        // Act
        var result = cssBuilder.AddClassFromAttributes(null).Build();

        // Assert
        result.Should().Be("value");
    }

    [Fact]
    public void ShouldBuildConditionalCssClasses()
    {
        // Arrange
        const bool hasTwo = false;
        const bool hasThree = true;
#pragma warning disable IDE0039 // Use local function
        Func<bool> hasFive = () => false;
#pragma warning restore IDE0039 // Use local function

        // Act
        var classToRender = new CssBuilder("item-one")
                        .AddClass("item-two", when: hasTwo)
                        .AddClass("item-three", when: hasThree)
                        .AddClass("item-four")
                        .AddClass("item-five", when: hasFive)
                        .Build();
        // Assert
        classToRender.Should().Be("item-one item-three item-four");
    }

    [Fact]
    public void ShouldBuildConditionalCssBuilderClasses()
    {
        // Arrange
        const bool hasTwo = false;
        const bool hasThree = true;
        static bool hasFive() => false;

        // Act
        var classToRender = new CssBuilder("item-one")
                        .AddClass("item-two", when: hasTwo)
                        .AddClass(new CssBuilder("item-three")
                                        .AddClass("item-foo", false)
                                        .AddClass("item-sub-three"),
                                        when: hasThree)
                        .AddClass("item-four")
                        .AddClass("item-five", when: hasFive)
                        .Build();
        // Assert
        classToRender.Should().Be("item-one item-three item-sub-three item-four");
    }

    [Fact]
    public void ShouldBuildEmptyClasses()
    {
        // Arrange
        const bool shouldShow = false;

        // Act
        var classToRender = new CssBuilder()
                        .AddClass("some-class", shouldShow)
                        .Build();
        // Assert
        classToRender.Should().Be(string.Empty);
    }

    [Fact]
    public void ShouldBuildClassesWithFunc()
    {
        // Arrange
        IReadOnlyDictionary<string, object> attributes = new Dictionary<string, object> { { "class", "my-custom-class-1" } };

        // Act
        var classToRender = new CssBuilder("item-one")
                        .AddClass(() => attributes["class"].ToString(), when: attributes.ContainsKey("class"))
                        .Build();
        // Assert
        classToRender.Should().Be("item-one my-custom-class-1");
    }

    [Fact]
    public void ShouldNotThrowWhenNullFor_BuildClassesFromAttributes()
    {
        // Arrange
        IReadOnlyDictionary<string, object>? attributes = null;

        // Act
        var classToRender = new CssBuilder("item-one")
                        .AddClassFromAttributes(attributes)
                        .Build();

        // Assert
        classToRender.Should().Be("item-one");
    }

    [Fact]
    public void AddClass_WithFuncValueAndFuncCondition_ShouldAddClassIfConditionIsTrue()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");

        // Act
        var result = cssBuilder.AddClass(() => "newClass", () => true).Build();

        // Assert
        result.Should().Be("value newClass");
    }

    [Fact]
    public void AddClass_WithFuncValueAndFuncCondition_ShouldNotAddClassIfConditionIsFalse()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");

        // Act
        var result = cssBuilder.AddClass(() => "newClass", () => false).Build();

        // Assert
        result.Should().Be("value");
    }

    [Fact]
    public void AddClass_WithFuncValueAndFuncCondition_ShouldNotAddClassIfConditionIsNull()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");

        // Act
        var result = cssBuilder.AddClass(() => "newClass", (Func<bool>?)null).Build();

        // Assert
        result.Should().Be("value");
    }

    [Fact]
    public void AddClass_WithCssBuilderAndFuncCondition_ShouldAddClassIfConditionIsTrue()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");
        var otherBuilder = new CssBuilder("otherClass");

        // Act
        var result = cssBuilder.AddClass(otherBuilder, () => true).Build();

        // Assert
        result.Should().Be("value otherClass");
    }

    [Fact]
    public void AddClass_WithCssBuilderAndFuncCondition_ShouldNotAddClassIfConditionIsFalse()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");
        var otherBuilder = new CssBuilder("otherClass");

        // Act
        var result = cssBuilder.AddClass(otherBuilder, () => false).Build();

        // Assert
        result.Should().Be("value");
    }

    [Fact]
    public void AddClass_WithCssBuilderAndFuncCondition_ShouldNotAddClassIfConditionIsNull()
    {
        // Arrange
        var cssBuilder = new CssBuilder("value");
        var otherBuilder = new CssBuilder("otherClass");

        // Act
        var result = cssBuilder.AddClass(otherBuilder, (Func<bool>?)null).Build();

        // Assert
        result.Should().Be("value");
    }
}
