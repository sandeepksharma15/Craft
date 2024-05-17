using Bunit;
using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;
using Craft.MediaQuery.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Moq;

namespace Craft.MediaQuery.Tests.Services;

public class ViewportResizeListenerServiceTests : TestContext
{
    [Fact]
    public async Task GetBreakpoint_ShouldInvokeJSInterop()
    {
        // Arrange
        JSInterop.SetupModule("import", ["./_content/Craft.MediaQuery/resizeListener.js"]);
        JSInterop
            .Setup<Breakpoint>("getBreakpoint", "084b2348")
            .SetResult(Breakpoint.Widescreen);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var listener = CreateViewportResizeListener(JSInterop.JSRuntime);

        // Act
        await listener.GetBreakpointAsync();

        // Assert
        JSInterop.VerifyInvoke("getBreakpoint");
    }

    [Fact]
    public async Task GetViewportSize_ShouldReturnExpectedResult()
    {
        // Arrange
        JSInterop.SetupModule("import", ["./_content/Craft.MediaQuery/resizeListener.js"]);
        JSInterop
            .Setup<ViewportSizeEventArgs>("getViewportSize", "084b2348")
            .SetResult(new ViewportSizeEventArgs { Height = 400, Width = 400 });
        JSInterop.Mode = JSRuntimeMode.Loose;

        var listener = CreateViewportResizeListener(JSInterop.JSRuntime);

        // Act
        await listener.GetViewportSizeAsync();

        // Assert
        JSInterop.VerifyInvoke("getViewportSize");
    }

    [Fact]
    public async Task IsBreakpointMatching_ShouldInvokeGetBreakpointIfNotCached()
    {
        // Arrange
        JSInterop.SetupModule("import", ["./_content/Craft.MediaQuery/resizeListener.js"]);
        JSInterop
            .Setup<Breakpoint>("getBreakpoint", "084b2348")
            .SetResult(Breakpoint.Widescreen);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var listener = CreateViewportResizeListener(JSInterop.JSRuntime);

        // Act
        await listener.IsBreakpointMatchingAsync(Breakpoint.Mobile);

        // Assert
        JSInterop.VerifyInvoke("getBreakpoint");
    }

    [Fact]
    public async Task MatchMedia_ShouldInvokeJSInterop()
    {
        // Arrange
        JSInterop.SetupModule("import", ["./_content/Craft.MediaQuery/resizeListener.js"]);
        JSInterop
            .Setup<bool>("matchMediaQuery", "someMediaQuery", "084b2348")
            .SetResult(true);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var listener = CreateViewportResizeListener(JSInterop.JSRuntime);

        // Act
        await listener.MatchMediaAsync("someMediaQuery");

        // Assert
        JSInterop.VerifyInvoke("matchMediaQuery");
    }

    private static ViewportResizeListener CreateViewportResizeListener(IJSRuntime jsRuntime = null)
    {
        var loggerMock = new Mock<ILogger<ViewportResizeListener>>();
        var optionsMock = Options.Create(new ResizeOptions());
        jsRuntime ??= new Mock<IJSRuntime>().Object;

        return new ViewportResizeListener(jsRuntime, loggerMock.Object, optionsMock);
    }

    // public async Task MatchMediaQuery_ShouldInvokeJSInteropForMinWidth()
    // public async Task MatchMediaQuery_ShouldInvokeJSInteropForMaxWidth()
}
