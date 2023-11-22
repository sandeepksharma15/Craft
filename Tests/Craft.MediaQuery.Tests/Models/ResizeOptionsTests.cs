using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;
using FluentAssertions;

namespace Craft.MediaQuery.Tests.Models;

public class ResizeOptionsTests
{
    [Fact]
    public void Equals_SameInstance_ShouldReturnTrue()
    {
        // Arrange
        var options = new ResizeOptions();

        // Act & Assert
        options.Should().Be(options);
    }

    [Fact]
    public void Equals_NullObject_ShouldReturnFalse()
    {
        // Arrange
        var options = new ResizeOptions();

        // Act & Assert
        options.Equals(null).Should().BeFalse();
    }

    [Fact]
    public void Equals_DifferentType_ShouldReturnFalse()
    {
        // Arrange
        var options = new ResizeOptions();

        // Act & Assert
        options.Equals(new object()).Should().BeFalse();
    }

    [Fact]
    public void Equals_EqualObjects_ShouldReturnTrue()
    {
        // Arrange
        var options1 = new ResizeOptions();
        var options2 = new ResizeOptions();

        // Act & Assert
        options1.Equals(options2).Should().BeTrue();
    }

    [Fact]
    public void Equals_DifferentReportRate_ShouldReturnFalse()
    {
        // Arrange
        var options1 = new ResizeOptions();
        var options2 = new ResizeOptions { ReportRate = 200 };

        // Act & Assert
        options1.Equals(options2).Should().BeFalse();
    }

    [Fact]
    public void Equals_DifferentEnableLogging_ShouldReturnFalse()
    {
        // Arrange
        var options1 = new ResizeOptions();
        var options2 = new ResizeOptions { EnableLogging = true };

        // Act & Assert
        options1.Equals(options2).Should().BeFalse();
    }

    [Fact]
    public void Equals_DifferentNotifyOnBreakpointOnly_ShouldReturnFalse()
    {
        // Arrange
        var options1 = new ResizeOptions();
        var options2 = new ResizeOptions { NotifyOnBreakpointOnly = false };

        // Act & Assert
        options1.Equals(options2).Should().BeFalse();
    }

    [Fact]
    public void Equals_DifferentBreakpoints_ShouldReturnFalse()
    {
        // Arrange
        var options1 = new ResizeOptions { Breakpoints = new Dictionary<Breakpoint, int> { { Breakpoint.ExtraSmall, 320 } } };
        var options2 = new ResizeOptions { Breakpoints = new Dictionary<Breakpoint, int> { { Breakpoint.Tablet, 768 } } };

        // Act & Assert
        options1.Equals(options2).Should().BeFalse();
    }

    [Fact]
    public void GetHashCode_SameInstance_ShouldBeEqual()
    {
        // Arrange
        var options = new ResizeOptions();

        // Act & Assert
        options.GetHashCode().Should().Be(options.GetHashCode());
    }

    [Fact]
    public void Clone_ShouldReturnEqualButDistinctInstance()
    {
        // Arrange
        var original = new ResizeOptions
        {
            ReportRate = 200,
            EnableLogging = true,
            SuppressFirstEvent = false,
            NotifyOnBreakpointOnly = false,
            Breakpoints = new Dictionary<Breakpoint, int> { { Breakpoint.ExtraSmall, 320 } }
        };

        // Act
        var clone = original.Clone();

        // Assert
        clone.Should().BeEquivalentTo(original);
        clone.Should().NotBeSameAs(original);
    }
}
