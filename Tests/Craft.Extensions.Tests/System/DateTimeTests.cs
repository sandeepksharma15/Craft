using FluentAssertions;

namespace Craft.Extensions.Tests.System;

public class DateTimeTests
{
    [Fact]
    public void ClearTime_Should_Not_Modify_Date_With_Time_Already_At_Midnight()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 25, 0, 0, 0, 0);

        // Act
        var result = dateTime.ClearTime();

        // Assert
        result.Should().Be(new DateTime(2023, 6, 25));
    }

    [Fact]
    public void ClearTime_Should_Not_Modify_Date_With_Time_Already_Set()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 25, 8, 30, 15, 500);

        // Act
        var result = dateTime.ClearTime();

        // Assert
        result.Should().Be(new DateTime(2023, 6, 25));
    }

    [Fact]
    public void ClearTime_Should_Set_Time_To_Midnight()
    {
        // Arrange
        var dateTime = new DateTime(2023, 6, 25, 15, 30, 45, 123);

        // Act
        var result = dateTime.ClearTime();

        // Assert
        result.Should().Be(new DateTime(2023, 6, 25));
    }

    [Theory]
    [InlineData(DayOfWeek.Monday, true)]
    [InlineData(DayOfWeek.Tuesday, true)]
    [InlineData(DayOfWeek.Wednesday, true)]
    [InlineData(DayOfWeek.Thursday, true)]
    [InlineData(DayOfWeek.Friday, true)]
    [InlineData(DayOfWeek.Saturday, false)]
    [InlineData(DayOfWeek.Sunday, false)]
    public void IsWeekDay_Should_Return_Correct_Result(DayOfWeek dayOfWeek, bool expectedResult)
    {
        // Act
        var result = dayOfWeek.IsWeekday();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(DayOfWeek.Monday, false)]
    [InlineData(DayOfWeek.Tuesday, false)]
    [InlineData(DayOfWeek.Wednesday, false)]
    [InlineData(DayOfWeek.Thursday, false)]
    [InlineData(DayOfWeek.Friday, false)]
    [InlineData(DayOfWeek.Saturday, true)]
    [InlineData(DayOfWeek.Sunday, true)]
    public void IsWeekend_Should_Return_Correct_Result(DayOfWeek dayOfWeek, bool expectedResult)
    {
        // Act
        var result = dayOfWeek.IsWeekend();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void LocalKindIsOverwrittenTest()
    {
        DateTime? input = DateTime.Now;
        DateTime withKindUtcInput = DateTime.SpecifyKind(input.Value, DateTimeKind.Local);
        DateTime? result = withKindUtcInput.SetKindUtc();

        result.Should().NotBeNull();
        result.Value.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void SetKindUtcNonNullOffsetDateInputTest()
    {
        DateTime? input = DateTime.Now;
        DateTime withKindUtcInput = DateTime.SpecifyKind(input.Value, DateTimeKind.Utc);
        DateTime? result = withKindUtcInput.SetKindUtc();
        result.Should().NotBeNull();
        result.Value.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void SetKindUtcNonNullRegularDateInputTest()
    {
        DateTime? input = DateTime.Now;
        DateTime? result = input.SetKindUtc();
        result.Should().NotBeNull();

        result.Value.Kind.Should().Be(DateTimeKind.Utc);
    }

    [Fact]
    public void SetKindUtcNullInputTest()
    {
        DateTime? input = null;
        DateTime? result = input.SetKindUtc();
        result.Should().BeNull();
    }

    [Fact]
    public void UnspecifiedKindIsOverwrittenTest()
    {
        DateTime? input = DateTime.Now;
        DateTime withKindUtcInput = DateTime.SpecifyKind(input.Value, DateTimeKind.Unspecified);
        DateTime? result = withKindUtcInput.SetKindUtc();

        result.Should().NotBeNull();
        result.Value.Kind.Should().Be(DateTimeKind.Utc);
    }
}
