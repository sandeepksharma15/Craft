using Craft.MediaQuery.Enums;

using FluentAssertions;

namespace Craft.MediaQuery.Tests.Enums;

public class BreakpointExtensionsTests
{
    #region Public Methods

    [Theory]
    [InlineData(Breakpoint.Mobile, Breakpoint.MobileAndUp, true)]
    [InlineData(Breakpoint.Tablet, Breakpoint.TabletAndUp, true)]
    [InlineData(Breakpoint.Desktop, Breakpoint.DesktopAndUp, true)]
    [InlineData(Breakpoint.Widescreen, Breakpoint.WidescreenAndUp, true)]
    public void IsMatchingWith_GreaterOrEqualBreakpoints_ShouldReturnTrue(Breakpoint one, Breakpoint another, bool expected)
    {
        // Act
        bool result = one.IsMatchingWith(another);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(Breakpoint.ExtraSmall, Breakpoint.MobileAndDown, true)]
    [InlineData(Breakpoint.Mobile, Breakpoint.TabletAndDown, true)]
    [InlineData(Breakpoint.Tablet, Breakpoint.DesktopAndDown, true)]
    [InlineData(Breakpoint.Desktop, Breakpoint.WidescreenAndDown, true)]
    public void IsMatchingWith_LowerOrEqualBreakpoints_ShouldReturnTrue(Breakpoint one, Breakpoint another, bool expected)
    {
        // Act
        bool result = one.IsMatchingWith(another);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(Breakpoint.Desktop, Breakpoint.None, false)]
    [InlineData(Breakpoint.Widescreen, Breakpoint.Always, true)]
    public void IsMatchingWith_OtherCases_ShouldReturnExpected(Breakpoint one, Breakpoint another, bool expected)
    {
        // Act
        bool result = one.IsMatchingWith(another);

        // Assert
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(Breakpoint.ExtraSmall, Breakpoint.ExtraSmall, true)]
    [InlineData(Breakpoint.Mobile, Breakpoint.Mobile, true)]
    [InlineData(Breakpoint.Tablet, Breakpoint.Tablet, true)]
    [InlineData(Breakpoint.Desktop, Breakpoint.Desktop, true)]
    [InlineData(Breakpoint.Widescreen, Breakpoint.Widescreen, true)]
    [InlineData(Breakpoint.FullHd, Breakpoint.FullHd, true)]
    public void IsMatchingWith_SameBreakpoints_ShouldReturnTrue(Breakpoint one, Breakpoint another, bool expected)
    {
        // Act
        bool result = one.IsMatchingWith(another);

        // Assert
        result.Should().Be(expected);
    }

    #endregion Public Methods
}
