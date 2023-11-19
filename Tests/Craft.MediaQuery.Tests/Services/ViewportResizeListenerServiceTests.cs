using Craft.MediaQuery.Services;
using Microsoft.JSInterop;
using Moq;
using Microsoft.Extensions.Logging;
using Craft.MediaQuery.Models;
using FluentAssertions;
using Bunit;
using Microsoft.Extensions.Options;

namespace Craft.MediaQuery.Tests.Services;

public class ViewportResizeListenerServiceTests : TestContext
{
    [Fact]
    public async Task GetViewportSize_ShouldReturnViewportSize()
    {
        // Arrange
        var moduleInterop = JSInterop.SetupModule("./_content/Craft.MediaQuery/resizeListener.js");
        moduleInterop.Setup<ViewportSize>("getViewportSize").SetResult(new ViewportSize { Width = 1000, Height = 1000 });

        // Setup Mock IjsRuntime
        var jsRuntimeMock = new Mock<IJSRuntime>();

        
    }
}
