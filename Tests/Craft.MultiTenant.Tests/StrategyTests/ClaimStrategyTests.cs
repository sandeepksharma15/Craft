using System.Security.Claims;
using Craft.MultiTenant.Strategies;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace Craft.MultiTenant.Tests.StrategyTests;

public class ClaimStrategyTests
{
    [Fact]
    public async Task GetIdentifierAsync_AuthenticatedUser_ReturnsIdentifierFromClaim()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var user = new ClaimsPrincipal(new ClaimsIdentity([
                new("tenantId", "myidentifier"),
                new(ClaimTypes.Authentication, "true")
            ], "Basic"));

        context.User = user;

        var strategy = new ClaimStrategy("tenantId");

        // Act
        var identifier = await strategy.GetIdentifierAsync(context);

        // Assert
        identifier.Should().Be("myidentifier");
    }

    [Fact]
    public async Task GetIdentifierAsync_NullHttpContext_ThrowsException()
    {
        // Arrange
        HttpContext context = null;

        var strategy = new ClaimStrategy("tenantId");

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => strategy.GetIdentifierAsync(context));
    }

    [Fact]
    public async Task GetIdentifierAsync_UnauthenticatedUser_ReturnsNull()
    {
        // Arrange
        var context = new DefaultHttpContext();

        var strategy = new ClaimStrategy("tenantId");

        // Act
        var identifier = await strategy.GetIdentifierAsync(context);

        // Assert
        identifier.Should().BeNull();
    }
}
