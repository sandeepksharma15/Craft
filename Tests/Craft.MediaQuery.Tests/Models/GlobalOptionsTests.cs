using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;
using FluentAssertions;

namespace Craft.MediaQuery.Tests.Models;

public class GlobalOptionsTests
{
    [Fact]
    public void GetDefaultBreakpoints_ShouldReturnDefaultBreakpoints()
    {
        // Arrange

        // Act
        var defaultBreakpoints = GlobalOptions.GetDefaultBreakpoints();

        // Assert
        defaultBreakpoints.Should().NotBeNull()
            .And.BeEquivalentTo(new Dictionary<Breakpoint, int>
            {
                [Breakpoint.FullHd] = 1920,
                [Breakpoint.Widescreen] = 1600,
                [Breakpoint.Desktop] = 1200,
                [Breakpoint.Tablet] = 900,
                [Breakpoint.Mobile] = 600,
                [Breakpoint.ExtraSmall] = 0,
            });
    }

    [Fact]
    public void GetBreakpoints_WithNonNullOptions_ShouldReturnOptionsBreakpoints()
    {
        // Arrange
        var options = new ResizeOptions
        {
            Breakpoints = new Dictionary<Breakpoint, int>
            {
                [Breakpoint.ExtraSmall] = 300,
                [Breakpoint.Mobile] = 500,
            }
        };

        // Act
        var breakpoints = GlobalOptions.GetBreakpoints(options);

        // Assert
        breakpoints.Should().NotBeNull()
            .And.BeEquivalentTo(options.Breakpoints);
    }

    [Fact]
    public void GetBreakpoints_WithNullOptions_ShouldReturnDefaultBreakpoints()
    {
        // Arrange

        // Act
        var breakpoints = GlobalOptions.GetBreakpoints(null);

        // Assert
        breakpoints.Should().NotBeNull()
            .And.BeEquivalentTo(GlobalOptions.DefaultBreakpoints);
    }

    [Fact]
    public void GetDefaultResizeOptions_ShouldReturnDefaultOptions()
    {
        // Arrange

        // Act
        var defaultOptions = GlobalOptions.GetDefaultResizeOptions();

        // Assert
        defaultOptions.Should().NotBeNull()
            .And.BeEquivalentTo(new ResizeOptions
            {
                Breakpoints = GlobalOptions.DefaultBreakpoints,
                EnableLogging = true,
                NotifyOnBreakpointOnly = true,
                ReportRate = 300,
                SuppressFirstEvent = false
            });
    }
}
