using Craft.MultiTenant.Strategies;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace Craft.MultiTenant.Tests.StrategyTests;

public class StaticStrategyTests
{
    [Fact]
    public async Task GetIdentifierAsync_ReturnsCorrectIdentifier()
    {
        // Arrange
        var strategy = new StaticStrategy("test");
        var context = new DefaultHttpContext();

        // Act
        var result = await strategy.GetIdentifierAsync(context);

        // Assert
        result.Should().Be("test");
    }

    [Fact]
    public void Priority_ReturnsExpectedPriority()
    {
        // Arrange
        var strategy = new StaticStrategy("test");

        // Act
        int priority = strategy.Priority;

        // Assert
        priority.Should().Be(-1000);
    }

    [Theory]
    [InlineData("initech")]
    [InlineData("Initech")] // maintain case
    public async Task ReturnExpectedIdentifier(string staticIdentifier)
    {
        var strategy = new StaticStrategy(staticIdentifier);

        var identifier = await strategy.GetIdentifierAsync(new DefaultHttpContext());

        Assert.Equal(staticIdentifier, identifier);
    }
}
