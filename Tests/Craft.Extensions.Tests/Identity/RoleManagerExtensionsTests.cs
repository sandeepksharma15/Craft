using System.Security.Claims;
using Craft.TestHelper;
using Craft.TestHelper.Fixtures;
using Craft.TestHelper.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Craft.Extensions.Tests.Identity;

[Collection(nameof(SystemTestCollectionDefinition))]
public class RoleManagerExtensionsTests
{
    private readonly RoleManager<TestRole> roleManager;
    private readonly TestDbContext dbContext;

    public RoleManagerExtensionsTests()
    {
        // Create a new service provider.
        var services = new ServiceCollection()
            .AddDbContext<TestDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDb");
            });

        services
            .AddIdentityCore<TestUser>()
            .AddRoles<TestRole>()
            .AddEntityFrameworkStores<TestDbContext>();

        var serviceProvider = services.BuildServiceProvider();

        // Get The DbContext instance
        dbContext = serviceProvider.GetRequiredService<TestDbContext>();

        // Ensure the database is created.
        dbContext.Database.EnsureCreated();

        // Get The RoleManager instance
        roleManager = serviceProvider.GetRequiredService<RoleManager<TestRole>>();
    }

    [Fact]
    public async Task AddAsync_ShouldCreateRoleAndReturnIt()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Name = "Foo" };

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
        dbContext.Database.EnsureCreated();
        var roleClaim = new IdentityRoleClaim<int> { RoleId = 100, ClaimType = "Permission", ClaimValue = "Read" };

        // Act
        var result = await roleManager.CreateRoleClaimAsync(roleClaim);

        // Assert
        result.Succeeded.Should().BeFalse();
    }

    [Fact]
    public async Task CreateRoleClaimAsync_WhenAddClaimAsyncSucceeds_ShouldReturnSuccessIdentityResult()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Name = "Foo" };
        await roleManager.AddAsync(role);
        var roleClaim = new IdentityRoleClaim<int> { RoleId = role.Id, ClaimType = "Permission", ClaimValue = "Read" };

        // Act
        var result = await roleManager.CreateRoleClaimAsync(roleClaim);

        // Assert
        result.Should().BeEquivalentTo(IdentityResult.Success);
    }

    [Fact]
    public async Task GetRoleClaimAsync_WithClaims_ReturnsClaim()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Id = 10, Name = "FooFoo" };
        role = await roleManager.AddAsync(role);

        const string claimType = "yourClaimType";
        const string claimValue = "yourClaimValue";
        await roleManager.AddClaimAsync(role, new Claim(claimType, claimValue));

        // Act
        var result = await roleManager.GetRoleClaimAsync(role, claimType);

        // Assert
        result.Should().NotBeNull();
        result.Type.Should().Be(claimType);
        result.Value.Should().Be(claimValue);
    }

    [Fact]
    public async Task GetRoleClaimAsync_WithRoleName_WithClaims_ReturnsClaim()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Id = 11, Name = "Fu" };
        role = await roleManager.AddAsync(role);

        const string claimType = "yourClaimType";
        const string claimValue = "yourClaimValue";
        await roleManager.AddClaimAsync(role, new Claim(claimType, claimValue));

        // Act
        var result = await roleManager.GetRoleClaimAsync(role.Name, claimType);

        // Assert
        result.Should().NotBeNull();
        result.Type.Should().Be(claimType);
        result.Value.Should().Be(claimValue);
    }

    [Fact]
    public async Task GetRoleClaimAsync_WithRoleId_WithClaims_ReturnsClaim()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Id = 13, Name = "Fuu" };
        role = await roleManager.AddAsync(role);

        const string claimType = "yourClaimType";
        const string claimValue = "yourClaimValue";
        await roleManager.AddClaimAsync(role, new Claim(claimType, claimValue));

        // Act
        var result = await roleManager.GetRoleClaimAsync(role.Id, claimType);

        // Assert
        result.Should().NotBeNull();
        result.Type.Should().Be(claimType);
        result.Value.Should().Be(claimValue);
    }

    [Fact]
    public async Task GetRoleClaimAsync_WithoutClaims_ReturnsNull()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Id = 22, Name = "FooFooFoo" };
        role = await roleManager.AddAsync(role);

        const string claimType = "yourClaimType";
        // Act
        var result = await roleManager.GetRoleClaimAsync(role, claimType);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetRoleClaimValueAsync_WithClaims_ReturnsClaimValue()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Id = 30, Name = "Fooo" };
        role = await roleManager.AddAsync(role);

        const string claimType = "yourClaimType";
        const string claimValue = "yourClaimValue";
        await roleManager.AddClaimAsync(role, new Claim(claimType, claimValue));

        // Act
        var result = await roleManager.GetRoleClaimValueAsync<TestRole>(role, claimType);

        // Assert
        result.Should().Be(claimValue);
    }

    [Fact]
    public async Task GetRoleClaimValueAsync_NoClaims_ReturnsNull()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Id = 40, Name = "Foooo" };
        role = await roleManager.AddAsync(role);

        const string claimType = "yourClaimType";

        // Act
        var result = await roleManager.GetRoleClaimValueAsync<TestRole>(role, claimType);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetRoleClaimValueAsync_WithRoleName_WithClaims_ReturnsClaimValue()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Id = 15, Name = "Fou" };
        role = await roleManager.AddAsync(role);

        const string claimType = "yourClaimType";
        const string claimValue = "yourClaimValue";
        await roleManager.AddClaimAsync(role, new Claim(claimType, claimValue));

        // Act
        var result = await roleManager.GetRoleClaimValueAsync<TestRole>(role.Name, claimType);

        // Assert
        result.Should().Be(claimValue);
    }

    [Fact]
    public async Task GetRoleClaimValueAsync_WithRoleId_WithClaims_ReturnsClaimValue()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Id = 17, Name = "Fuou" };
        role = await roleManager.AddAsync(role);

        const string claimType = "yourClaimType";
        const string claimValue = "yourClaimValue";
        await roleManager.AddClaimAsync(role, new Claim(claimType, claimValue));

        // Act
        var result = await roleManager.GetRoleClaimValueAsync<TestRole, int>(role.Id, claimType);

        // Assert
        result.Should().Be(claimValue);
    }

    [Fact]
    public async Task RemoveRoleClaimAsync_WithValidRoleClaim_RemovesClaim()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Id = 19, Name = "Fuouo" };
        role = await roleManager.AddAsync(role);

        const string claimType = "yourClaimType";
        const string claimValue = "yourClaimValue";
        var roleClaim = new IdentityRoleClaim<int>() { RoleId = role.Id, ClaimType = claimType, ClaimValue = claimValue };
        await roleManager.CreateRoleClaimAsync(roleClaim);

        // Act
        var result = await roleManager.RemoveRoleClaimAsync(roleClaim);

        // Assert
        result.Succeeded.Should().BeTrue();
        var claims = await roleManager.GetClaimsAsync(role);
        claims.Should().NotContain(c => c.Type == roleClaim.ClaimType && c.Value == roleClaim.ClaimValue);
    }

    [Fact]
    public async Task RemoveRoleClaimAsync_WithNoClaims_ReturnsFailedResult()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Id = 21, Name = "Fuuu" };
        role = await roleManager.AddAsync(role);

        dbContext.Database.EnsureCreated();
        const string claimType = "yourClaimType";
        const string claimValue = "yourClaimValue";
        var roleClaim = new IdentityRoleClaim<int>() { RoleId = role.Id, ClaimType = claimType, ClaimValue = claimValue };

        // Act
        var result = await roleManager.RemoveRoleClaimAsync(roleClaim);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Description.Contains(role.Id.ToString()));
    }

    [Fact]
    public async Task RemoveRoleClaimAsync_WithNonexistentRole_ReturnsFailedResult()
    {
        // Arrange
        const int roleId = 100;

        dbContext.Database.EnsureCreated();
        const string claimType = "yourClaimType";
        const string claimValue = "yourClaimValue";
        var roleClaim = new IdentityRoleClaim<int>() { RoleId = roleId, ClaimType = claimType, ClaimValue = claimValue };
        //await roleManager.CreateRoleClaimAsync(roleClaim);

        // Act
        var result = await roleManager.RemoveRoleClaimAsync(roleClaim);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Description.Contains(roleId.ToString()));
    }

    [Fact]
    public async Task RemoveRoleClaimAsync_WithNonExistentClaims_ReturnsFailedResult()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Id = 23, Name = "Fuuuu" };
        role = await roleManager.AddAsync(role);

        dbContext.Database.EnsureCreated();
        const string claimType = "yourClaimType";
        const string claimValue = "yourClaimValue";
        var roleClaim = new IdentityRoleClaim<int>() { RoleId = role.Id, ClaimType = claimType, ClaimValue = claimValue };
        await roleManager.CreateRoleClaimAsync(roleClaim);

        var testRoleClaim = new IdentityRoleClaim<int>() { RoleId = role.Id, ClaimType = "None", ClaimValue = "None"};

        // Act
        var result = await roleManager.RemoveRoleClaimAsync(testRoleClaim);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Description.Contains(role.Id.ToString()));
    }

    [Fact]
    public async Task UpdateRoleClaimAsync_WithValidRoleClaim_RemovesClaim()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Id = 27, Name = "Fouo" };
        role = await roleManager.AddAsync(role);

        const string claimType = "yourClaimType";
        const string claimValue = "yourClaimValue";
        const string newClaimValue = "newClaimValue";

        var roleClaim = new IdentityRoleClaim<int>() { RoleId = role.Id, ClaimType = claimType, ClaimValue = claimValue };
        await roleManager.CreateRoleClaimAsync(roleClaim);
        roleClaim = new IdentityRoleClaim<int>() { RoleId = role.Id, ClaimType = claimType, ClaimValue = newClaimValue };

        // Act
        var result = await roleManager.UpdateRoleClaimAsync(roleClaim);

        // Assert
        result.Succeeded.Should().BeTrue();
        var claims = await roleManager.GetClaimsAsync(role);
        claims.Should().Contain(c => c.Type == roleClaim.ClaimType && c.Value == roleClaim.ClaimValue);
    }

    [Fact]
    public async Task UpdateRoleClaimAsync_WithNoClaims_ReturnsFailedResult()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Id = 29, Name = "Fouuu" };
        role = await roleManager.AddAsync(role);

        dbContext.Database.EnsureCreated();
        const string claimType = "yourClaimType";
        const string claimValue = "yourClaimValue";
        var roleClaim = new IdentityRoleClaim<int>() { RoleId = role.Id, ClaimType = claimType, ClaimValue = claimValue };

        // Act
        var result = await roleManager.UpdateRoleClaimAsync(roleClaim);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Description.Contains(role.Id.ToString()));
    }

    [Fact]
    public async Task UpdateRoleClaimAsync_WithNonexistentRole_ReturnsFailedResult()
    {
        // Arrange
        const int roleId = 100;

        dbContext.Database.EnsureCreated();
        const string claimType = "yourClaimType";
        const string claimValue = "yourClaimValue";
        var roleClaim = new IdentityRoleClaim<int>() { RoleId = roleId, ClaimType = claimType, ClaimValue = claimValue };
        //await roleManager.CreateRoleClaimAsync(roleClaim);

        // Act
        var result = await roleManager.UpdateRoleClaimAsync(roleClaim);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Description.Contains(roleId.ToString()));
    }

    [Fact]
    public async Task UpdateRoleClaimAsync_WithNonExistentClaims_ReturnsFailedResult()
    {
        // Arrange
        dbContext.Database.EnsureCreated();
        var role = new TestRole { Id = 31, Name = "Fuuuu" };
        role = await roleManager.AddAsync(role);

        dbContext.Database.EnsureCreated();
        const string claimType = "yourClaimType";
        const string claimValue = "yourClaimValue";
        var roleClaim = new IdentityRoleClaim<int>() { RoleId = role.Id, ClaimType = claimType, ClaimValue = claimValue };
        await roleManager.CreateRoleClaimAsync(roleClaim);

        var testRoleClaim = new IdentityRoleClaim<int>() { RoleId = role.Id, ClaimType = "None", ClaimValue = "None" };

        // Act
        var result = await roleManager.UpdateRoleClaimAsync(testRoleClaim);

        // Assert
        result.Succeeded.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.Description.Contains(role.Id.ToString()));
    }
}
