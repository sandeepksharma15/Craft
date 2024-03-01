using Craft.Components.Generic;
using Bunit;
using FluentAssertions;
using Xunit;

namespace Craft.Components.Tests.Generic;

public class SpinLoaderTests : TestContext
{
    [Fact]
    public void Display_ContentTemplate_If_IsLoading_IsFalse_And_IsFaulted_IsFalse()
    {
        //Arrange
        using var cut = RenderComponent<SpinLoader>(parameters => parameters
            .Add(p => p.IsLoading, false)
            .Add(p => p.IsFaulted, false)
            .Add(p => p.ContentTemplate, "<div>Content Loaded.</div>"));

        //Assert
        cut.Find("div").TextContent.Should().Be("Content Loaded.");
    }

    [Fact]
    public void Display_FaultedTemplate_If_IsLoading_IsFalse_And_IsFaulted_IsTrue()
    {
        //Arrange
        using var cut = RenderComponent<SpinLoader>(parameters => parameters
            .Add(p => p.IsLoading, false)
            .Add(p => p.IsFaulted, true));

        //Assert
        cut.Find("div").ClassList.Contains("mud-alert").Should().BeTrue();
    }

    [Fact]
    public void Display_Loading_Template_If_IsLoading_IsTrue()
    {
        // Arrange
        using var cut = RenderComponent<SpinLoader>(parameters =>
            parameters
                .Add(p => p.IsLoading, true));

        // Assert
        cut.Find("div").ClassList.Contains("mud-progress-circular").Should().BeTrue();
    }

    [Fact]
    public void Display_Loading_Template_If_IsLoading_IsTrue_When_LoadingTemplate_IsProvided()
    {
        //Arrange
        using var cut = RenderComponent<SpinLoader>(parameters => parameters
            .Add(p => p.IsLoading, true)
            .Add(p => p.LoadingTemplate, "<div>Loading...</div>"));

        //Assert
        cut.Find("div").TextContent.Should().Be("Loading...");
    }
}
