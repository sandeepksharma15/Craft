using Craft.TestHelper;
using Craft.TestHelper.Fixtures;
using Craft.TestHelper.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Craft.Extensions.Tests.Identity;

[Collection(nameof(SystemTestCollectionDefinition))]
public class UserManagerExtensionsTests
{
    private readonly TestDbContext dbContext;
    private readonly UserManager<TestUser> userManager;

    public UserManagerExtensionsTests()
    {
        // Create a new service provider.
        var services = new ServiceCollection()
            .AddDbContext<TestDbContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryTestDb");
            });

        services.AddDataProtection();

        services
            .AddIdentityCore<TestUser>()
            .AddDefaultTokenProviders()
            .AddRoles<TestRole>()
            .AddEntityFrameworkStores<TestDbContext>();

        var serviceProvider = services.BuildServiceProvider();

        // Get The DbContext instance
        dbContext = serviceProvider.GetRequiredService<TestDbContext>();

        // Ensure the database is created.
        dbContext.Database.EnsureCreated();

        // Get The UserManager instance
        userManager = serviceProvider.GetRequiredService<UserManager<TestUser>>();
    }

    [Fact]
    public async Task ConfirmPhoneNumberAsync_Success()
    {
        // Arrange
        var testUser = new TestUser { Id = 1, UserName = "TestUser", PhoneNumber = "1234567890" };

        await userManager.CreateAsync(testUser);

        // Act
        var user = await userManager.FindByIdAsync(1.ToString());
        var token = await userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);

        var result = await userManager.ConfirmPhoneNumberAsync(1, token);

        // Assert
        result.Should().BeEquivalentTo(IdentityResult.Success);
    }

    [Fact]
    public async Task ConfirmPhoneNumberAsync_UserNotFound()
    {
        // Arrange
        var testUser = new TestUser { Id = 2, UserName = "NextTestUser", PhoneNumber = "1234567890" };

        await userManager.CreateAsync(testUser);

        // Act
        var user = await userManager.FindByIdAsync(1.ToString());
        var token = await userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber);

        var result = await userManager.ConfirmPhoneNumberAsync(2, token);

        // Assert
        result.Succeeded.Should().BeFalse();
    }
}
