using Microsoft.JSInterop;
using Moq;
using Craft.MediaQuery.Models;
using Bunit;
using Craft.MediaQuery.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FluentAssertions;
using Microsoft.JSInterop.Implementation;
using Craft.MediaQuery.Enums;

namespace Craft.MediaQuery.Tests.Services;

public class ViewportResizeListenerServiceTests : TestContext
{
    private static ViewportResizeListener CreateViewportResizeListener(IJSRuntime jsRuntime = null)
    {
        var loggerMock = new Mock<ILogger<ViewportResizeListener>>();
        var optionsMock = Options.Create(new ResizeOptions());
        jsRuntime ??= new Mock<IJSRuntime>().Object;

        return new ViewportResizeListener(jsRuntime, loggerMock.Object, optionsMock);
    }

    [Fact]
    public async Task GetViewportSize_ShouldReturnExpectedResult()
    {
        // Arrange
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.MediaQuery/resizeListener.js" });
        JSInterop
            .Setup<ViewportSize>("getViewportSize", "084b2348")
            .SetResult(new ViewportSize { Height = 400, Width = 400 });
        JSInterop.Mode = JSRuntimeMode.Loose;

        var listener = CreateViewportResizeListener(JSInterop.JSRuntime);

        // Act
        await listener.GetViewportSize();

        // Assert
        JSInterop.VerifyInvoke("getViewportSize");
    }

    [Fact]
    public async Task GetBreakpoint_ShouldInvokeJSInterop()
    {
        // Arrange
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.MediaQuery/resizeListener.js" });
        JSInterop
            .Setup<Breakpoint>("getBreakpoint", "084b2348")
            .SetResult(Breakpoint.Widescreen);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var listener = CreateViewportResizeListener(JSInterop.JSRuntime);

        // Act
        await listener.GetBreakpoint();

        // Assert
        JSInterop.VerifyInvoke("getBreakpoint");
    }

    [Fact]
    public async Task MatchMedia_ShouldInvokeJSInterop()
    {
        // Arrange
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.MediaQuery/resizeListener.js" });
        JSInterop
            .Setup<bool>("matchMediaQuery", "someMediaQuery",  "084b2348")
            .SetResult(true);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var listener = CreateViewportResizeListener(JSInterop.JSRuntime);

        // Act
        await listener.MatchMedia("someMediaQuery");

        // Assert
        JSInterop.VerifyInvoke("matchMediaQuery");
    }

    [Fact]
    public void AreBreakpointsMatching_ShouldReturnTrueForEqualBreakpoints()
    {
        // Arrange
        var listener = CreateViewportResizeListener();

        // Act
        var result = listener.AreBreakpointsMatching(Breakpoint.Mobile, Breakpoint.Mobile);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task IsBreakpointMatching_ShouldInvokeGetBreakpointIfNotCached()
    {
        // Arrange
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.MediaQuery/resizeListener.js" });
        JSInterop
            .Setup<Breakpoint>("getBreakpoint", "084b2348")
            .SetResult(Breakpoint.Widescreen);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var listener = CreateViewportResizeListener(JSInterop.JSRuntime);

        // Act
        await listener.IsBreakpointMatching(Breakpoint.Mobile);

        // Assert
        JSInterop.VerifyInvoke("getBreakpoint");
    }

    // public async Task MatchMediaQuery_ShouldInvokeJSInteropForMinWidth()
    // public async Task MatchMediaQuery_ShouldInvokeJSInteropForMaxWidth()
}
