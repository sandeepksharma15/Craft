using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Test;
using Moq;

namespace Craft.Extensions.Tests.Fixtures;

public class TestRole : IdentityRole<int>
{
}

public class RoleManagerExtensionsTests
{
    [Fact]
    public async Task AddAsync_ShouldCreateRoleAndReturnIt()
    {
        // Arrange
        var normalizer = MockHelpers.MockLookupNormalizer();
        var store = new Mock<IRoleStore<TestRole>>();
        var role = new TestRole { Name = "Foo" };
        store.Setup(s => s.CreateAsync(role, CancellationToken.None)).ReturnsAsync(IdentityResult.Success).Verifiable();
        store.Setup(s => s.GetRoleNameAsync(role, CancellationToken.None)).Returns(Task.FromResult(role.Name)).Verifiable();
        store.Setup(s => s.SetNormalizedRoleNameAsync(role, normalizer.NormalizeName(role.Name), CancellationToken.None)).Returns(Task.FromResult(0)).Verifiable();
        var roleManager = MockHelpers.TestRoleManager(store.Object);

        // Act
        var result = await roleManager.AddAsync(role);

        // Assert
        result.Should().NotBeNull();
        result.Should().Be(role);
    }

    [Fact]
    public async Task CreateRoleClaimAsync_WhenRoleDoesNotExist_ShouldReturnFailedIdentityResult()
    {
        // Arrange
        var normalizer = MockHelpers.MockLookupNormalizer();
        var store = new Mock<IRoleStore<TestRole>>();
        var role = new TestRole { Name = "Foo" };
        var roleClaim = new IdentityRoleClaim<int> { RoleId = 1, ClaimType = "Permission", ClaimValue = "Read" };
        store.Setup(s => s.FindByIdAsync(roleClaim.RoleId.ToString(), CancellationToken.None))
            .ReturnsAsync((TestRole)null);
        var roleManager = MockHelpers.TestRoleManager(store.Object);

        // Act
        var result = await roleManager.CreateRoleClaimAsync(roleClaim);

        // Assert
        result.Succeeded.Should().BeFalse();
    }
}
