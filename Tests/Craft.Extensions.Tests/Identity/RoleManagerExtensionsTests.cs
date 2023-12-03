using Craft.Extensions.Tests.Fixtures;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Test;
using Moq;

namespace Craft.Extensions.Tests.Fixtures;

public class RoleManagerExtensionsTests
{
    [Fact]
    public async Task AddAsync_ShouldCreateRoleAndReturnIt()
    {
        // Arrange
        var normalizer = MockHelpers.MockLookupNormalizer();
        var store = new Mock<IRoleStore<PocoRole>>();
        var role = new PocoRole { Name = "Foo" };
        store.Setup(s => s.CreateAsync(role, CancellationToken.None)).ReturnsAsync(IdentityResult.Success).Verifiable();
        store.Setup(s => s.GetRoleNameAsync(role, CancellationToken.None)).Returns(Task.FromResult(role.Name)).Verifiable();
        store.Setup(s => s.SetNormalizedRoleNameAsync(role, normalizer.NormalizeName(role.Name), CancellationToken.None)).Returns(Task.FromResult(0)).Verifiable();
        var roleManager = MockHelpers.TestRoleManager(store.Object);

        // Act
        var result = await roleManager.AddAsync<PocoRole>(role);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(role);
    }
}
