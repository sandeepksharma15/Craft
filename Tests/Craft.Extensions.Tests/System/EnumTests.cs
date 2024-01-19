using System.ComponentModel;
using FluentAssertions;

namespace Craft.Extensions.Tests.System;

[Flags]
public enum TestEnum
{
    [Description("First description")]
    First = 0,

    [Description("Second description")]
    Second = 1,

    Third = 2
}

[Flags]
public enum SomeEnum
{
    Apple = 0,
    Orange = 1,
    Banana = 2
}

public class EnumTests
{
    public static IEnumerable<object[]> EnumValuesTestData =>
       [
                [TestEnum.First, "First", "First description"],
                [TestEnum.Second, "Second", "Second description"],
                [TestEnum.Third, "Third", "Third"],
       ];

    [Fact]
    public void GetDescription_ShouldReturnDescriptionAttribute_WhenEnumHasOne()
    {
        // Arrange
        const TestEnum someEnum = TestEnum.First;

        // Act
        string description = someEnum.GetDescription();

        // Assert
        description.Should().Be("First description");
    }

    [Fact]
    public void GetDescription_ShouldReturnEnumToString_WhenEnumHasNoDescriptionAttribute()
    {
        // Arrange
        const TestEnum someEnum = TestEnum.Third;

        // Act
        string description = someEnum.GetDescription();

        // Assert
        description.Should().Be("Third");
    }

    [Fact]
    public void GetDescription_ShouldReturnEmptyString_WhenArgumentIsNotAnEnum()
    {
        // Arrange
        const int notAnEnum = 123;

        // Act
        string description = notAnEnum.GetDescription();

        // Assert
        description.Should().BeEmpty();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(42)]
    public void ToEnum_ValuesExist_ShouldReturnCorrectValue(int value)
    {
        // Arrange
        TestEnum expected = (TestEnum)value;

        // Act
        TestEnum actual = value.ToEnum<TestEnum>();

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToEnum_Should_Throw_Exception_For_Non_Enum_Type()
    {
        // Arrange
        const int value = 4;

        // Act
        Action action = () => value.ToEnum<int>();

        // Assert
        action.Should().Throw<Exception>();
    }

    [Theory]
    [InlineData("Apple", SomeEnum.Apple, true)]
    [InlineData("Orange", SomeEnum.Apple | SomeEnum.Orange, true)]
    [InlineData("Banana", SomeEnum.Apple | SomeEnum.Orange, false)]
    [InlineData("orange", SomeEnum.Orange, true)]
#pragma warning disable RCS1257 // Use enum field explicitly.
    [InlineData("Orange", (SomeEnum)3, true)]
    public void Contains_ShouldReturnExpectedResult_WhenCalledWithValidInputs(string agent, SomeEnum flags, bool expected)
    {
        // Arrange
        // Act
        var actual = agent.Contains(flags);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ToStringInvariant_Should_ReturnEnumName()
    {
        // Arrange
        const TestEnum value = TestEnum.Second;

        // Act
        var result = value.ToStringInvariant();

        // Assert
        Assert.Equal("Second", result);
    }

    [Fact]
    public void GetFlags_Should_ReturnEnumFlags()
    {
        // Arrange
        const TestEnum value = TestEnum.First | TestEnum.Second;

        // Act
        var result = value.GetFlags();

        // Assert
        Assert.Contains(TestEnum.First, result);
        Assert.Contains(TestEnum.Second, result);
    }

    [Fact]
    public void IsSet_Should_ReturnTrue_WhenEnumContainsMatchingFlag()
    {
        // Arrange
        const TestEnum input = TestEnum.First | TestEnum.Second;
        const TestEnum matchTo = TestEnum.Second;

        // Act
        var result = input.IsSet(matchTo);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData("First", true, TestEnum.First)]
    [InlineData("first", true, TestEnum.First)]
    [InlineData("First", false, TestEnum.First)]
    [InlineData("Second", true, TestEnum.Second)]
    [InlineData("SECOND", true, TestEnum.Second)]
    [InlineData("Second", false, TestEnum.Second)]
    [InlineData("Third", true, TestEnum.Third)]
    public void ToEnum_ShouldConvertStringToEnum(string value, bool ignoreCase, TestEnum expectedEnum, string testCase = null)
    {
        // Act
        Action act = () => value.ToEnum<TestEnum>(ignoreCase).Should().Be(expectedEnum);

        // Assert
        act.Should().NotThrow(because: $"{testCase}");
    }

    [Theory]
    [MemberData(nameof(EnumValuesTestData))]
    public void GetName_ReturnsCorrectName(TestEnum value, string expectedName, string _)
    {
        string actualName = EnumValues<TestEnum>.GetName(value);
        actualName.Should().Be(expectedName);
    }

    [Theory]
    [MemberData(nameof(EnumValuesTestData))]
    public void GetDescription_ReturnsCorrectDescription(TestEnum value, string _, string expectedDescription)
    {
        string actualDescription = EnumValues<TestEnum>.GetDescription(value);
        actualDescription.Should().Be(expectedDescription);
    }

    [Fact]
    public void GetNames_ReturnsAllNames()
    {
        var expectedNames = new Dictionary<TestEnum, string>
        {
            { TestEnum.First, "First" },
            { TestEnum.Second, "Second" },
            { TestEnum.Third, "Third" },
        };

        var actualNames = EnumValues<TestEnum>.GetNames();
        actualNames.Should().BeEquivalentTo(expectedNames);
    }

    [Fact]
    public void GetDescriptions_ReturnsAllDescriptions()
    {
        var expectedDescriptions = new Dictionary<TestEnum, string>
        {
            { TestEnum.First, "First description" },
            { TestEnum.Second, "Second description" },
            { TestEnum.Third, "Third" },
        };

        var actualDescriptions = EnumValues<TestEnum>.GetDescriptions();
        actualDescriptions.Should().BeEquivalentTo(expectedDescriptions);
    }

    [Fact]
    public void GetValues_ReturnsAllValues()
    {
        var expectedValues = new[] { TestEnum.First, TestEnum.Second, TestEnum.Third };
        var actualValues = EnumValues<TestEnum>.GetValues();
        actualValues.Should().BeEquivalentTo(expectedValues);
    }

    [Theory]
    [MemberData(nameof(EnumValuesTestData))]
    public void TryGetSingleName_ReturnsTrueAndCorrectName(TestEnum value, string expectedName, string _)
    {
        bool result = EnumValues<TestEnum>.TryGetSingleName(value, out string actualName);
        result.Should().BeTrue();
        actualName.Should().Be(expectedName);
    }

    [Theory]
    [MemberData(nameof(EnumValuesTestData))]
    public void TryGetSingleDescription_ReturnsTrueAndCorrectDescription(TestEnum value, string _, string expectedDescription)
    {
        bool result = EnumValues<TestEnum>.TryGetSingleDescription(value, out string actualDescription);
        result.Should().BeTrue();
        actualDescription.Should().Be(expectedDescription);
    }
}
