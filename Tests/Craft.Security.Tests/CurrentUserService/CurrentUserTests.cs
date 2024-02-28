using Craft.Security.Claims;
using Craft.Security.CurrentUserService;
using FluentAssertions;
using Moq;
using System.Security.Claims;
using Xunit;

namespace Craft.Security.Tests.CurrentUserService;

public class CurrentUserTests
{
    [Fact]
    public void Constructor_SetsIdCorrectly()
    {
        // Arrange
        var mockProvider = new Mock<ICurrentUserProvider>();
        mockProvider.Setup(p => p.GetUser()).Returns(new ClaimsPrincipal(
                new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, "1")])));

        // Act
        var currentUser = new CurrentUser<int>(mockProvider.Object);

        // Assert
        currentUser.Id.Should().Be(1);
    }

    [Fact]
    public void NullProvider_SetsDefaultId()
    {
        // Arrange
        // Act
        var currentUser = new CurrentUser<int>(null);

        // Assert
        currentUser.Id.Should().Be(default);
    }

    [Fact]
    public void IsAuthenticated_AuthenticatedUser_ReturnsTrue()
    {
        // Arrange
        var mockProvider = new Mock<ICurrentUserProvider>();
        mockProvider.Setup(p => p.GetUser()).Returns(new ClaimsPrincipal(
            new ClaimsIdentity([new Claim(ClaimTypes.Name, "Test User")], "Basic")));

        // Act
        var currentUser = new CurrentUser<string>(mockProvider.Object);

        // Assert
        currentUser.IsAuthenticated().Should().BeTrue();
    }

    [Fact]
    public void IsAuthenticated_NonAuthenticatedUser_ReturnsFalse()
    {
        // Arrange
        var mockProvider = new Mock<ICurrentUserProvider>();
        mockProvider.Setup(p => p.GetUser()).Returns(new ClaimsPrincipal(
            new ClaimsIdentity([new Claim(ClaimTypes.Name, "Test User")])));

        // Act
        var currentUser = new CurrentUser<string>(mockProvider.Object);

        // Assert
        currentUser.IsAuthenticated().Should().BeFalse();
    }

    [Fact]
    public void GetName_AuthenticatedUser_RetrievesName()
    {
        // Arrange
        var mockProvider = new Mock<ICurrentUserProvider>();
        mockProvider.Setup(p => p.GetUser()).Returns(new ClaimsPrincipal(
            new ClaimsIdentity([new Claim(ClaimTypes.Name, "Test User")], "Basic")));

        // Act
        var currentUser = new CurrentUser<string>(mockProvider.Object);
        var name = currentUser.Name;

        // Assert
        name.Should().Be("Test User");
    }

    [Fact]
    public void GetName_NonAuthenticatedUser_ReturnsEmpty()
    {
        // Arrange
        var mockProvider = new Mock<ICurrentUserProvider>();
        mockProvider.Setup(p => p.GetUser()).Returns(new ClaimsPrincipal(
            new ClaimsIdentity([new Claim(ClaimTypes.Name, "Test User")])));

        // Act
        var currentUser = new CurrentUser<string>(mockProvider.Object);
        var name = currentUser.Name;

        // Assert
        name.Should().BeEmpty();
    }

    [Fact]
    public void GetUserClaims_AuthenticatedUser_ReturnsClaims()
    {
        // Arrange
        var mockProvider = new Mock<ICurrentUserProvider>();
        mockProvider.Setup(p => p.GetUser()).Returns(new ClaimsPrincipal(
            new ClaimsIdentity([new Claim(ClaimTypes.Name, "Test User")], "Basic")));

        // Act
        var currentUser = new CurrentUser<string>(mockProvider.Object);
        var claims = currentUser.GetUserClaims();

        // Assert
        claims.Should().NotBeNull().And.HaveCount(1);
        claims.First().Type.Should().Be(ClaimTypes.Name);
    }

    [Fact]
    public void GetUserClaims_NonAuthenticatedUser_ReturnsEmptyEnumerable()
    {
        // Arrange
        var mockProvider = new Mock<ICurrentUserProvider>();
        mockProvider.Setup(p => p.GetUser()).Returns(new ClaimsPrincipal(
            new ClaimsIdentity([new Claim(ClaimTypes.Name, "Test User")])));

        // Act
        var currentUser = new CurrentUser<string>(mockProvider.Object);
        var claims = currentUser.GetUserClaims();

        // Assert
        claims.Should().BeEmpty();
    }

    [Fact]
    public void IsInRole_ExistingRole_ReturnsTrue()
    {
        // Arrange
        var mockProvider = new Mock<ICurrentUserProvider>();
        mockProvider.Setup(p => p.GetUser()).Returns(new ClaimsPrincipal(
            new ClaimsIdentity([
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Role, "admin")
                ], "Basic")));

        // Act
        var currentUser = new CurrentUser<string>(mockProvider.Object);
        var isInRole = currentUser.IsInRole("admin");

        // Assert
        isInRole.Should().BeTrue();
    }

    [Fact]
    public void GetEmail_AuthenticatedUser_RetrievesEmail()
    {
        // Arrange
        var mockProvider = new Mock<ICurrentUserProvider>();
        mockProvider.Setup(p => p.GetUser()).Returns(new ClaimsPrincipal(
            new ClaimsIdentity([
                new Claim(ClaimTypes.Name, "Test User"),
                new Claim(ClaimTypes.Email, "Test Email")
                ], "Basic")));

        // Act
        var currentUser = new CurrentUser<string>(mockProvider.Object);
        var email = currentUser.GetEmail();

        // Assert
        email.Should().Be("Test Email");
    }

    [Fact]
    public void GetFirstName_AuthenticatedUser_RetrievesFirstName()
    {
        // Arrange
        var mockProvider = new Mock<ICurrentUserProvider>();
        mockProvider.Setup(p => p.GetUser()).Returns(new ClaimsPrincipal(
            new ClaimsIdentity([
                new Claim(ClaimTypes.Name, "Test User")
                ], "Basic")));

        // Act
        var currentUser = new CurrentUser<string>(mockProvider.Object);
        var firstName = currentUser.GetFirstName();

        // Assert
        firstName.Should().Be("Test User");
    }

    [Fact]
    public void GetFullName_AuthenticatedUser_RetrievesFullName()
    {
        // Arrange
        var mockProvider = new Mock<ICurrentUserProvider>();
        mockProvider.Setup(p => p.GetUser()).Returns(new ClaimsPrincipal(
            new ClaimsIdentity([
                new Claim(CraftClaims.Fullname, "Test User")
                ], "Basic")));

        // Act
        var currentUser = new CurrentUser<string>(mockProvider.Object);
        var fullName = currentUser.GetFullName();

        // Assert
        fullName.Should().Be("Test User");
    }

    [Fact]
    public void GetOtherClaims_AuthenticatedUser_RetrievesClaims()
    {
        // Arrange
        var mockProvider = new Mock<ICurrentUserProvider>();
        mockProvider.Setup(p => p.GetUser()).Returns(new ClaimsPrincipal(
            new ClaimsIdentity([
                new Claim(CraftClaims.ImageUrl, "Image Url"),
                new Claim(CraftClaims.JwtToken, "Jwt Token"),
                new Claim(ClaimTypes.Surname, "Last Name"),
                new Claim(ClaimTypes.MobilePhone, "Mobile Number"),
                new Claim(CraftClaims.Permissions, "Permissions"),
                new Claim(CraftClaims.Role, "Role"),
                new Claim(CraftClaims.Tenant, "Tenant")
                ], "Basic")));

        // Act
        var currentUser = new CurrentUser<string>(mockProvider.Object);
        var imageUrl = currentUser.GetImageUrl();
        var jwtToken = currentUser.GetJwtToken();
        var lastName = currentUser.GetLastName();
        var mobileNumber = currentUser.GetPhoneNumber();
        var permissions = currentUser.GetPermissions();
        var role = currentUser.GetRole();
        var tenant = currentUser.GetTenant();

        // Assert
        imageUrl.Should().Be("Image Url");
        jwtToken.Should().Be("Jwt Token");
        lastName.Should().Be("Last Name");
        mobileNumber.Should().Be("Mobile Number");
        permissions.Should().Be("Permissions");
        role.Should().Be("Role");
        tenant.Should().Be("Tenant");
    }

    [Fact]
    public void SetId_AuthenticatedUser_SetsUserId()
    {
        // Arrange
        var mockProvider = new Mock<ICurrentUserProvider>();
        mockProvider.Setup(p => p.GetUser()).Returns(new ClaimsPrincipal(
            new ClaimsIdentity([
                new Claim(CraftClaims.Fullname, "Test User")
                ], "Basic")));

        // Act
        var currentUser = new CurrentUser<string>(mockProvider.Object);
        currentUser.SetCurrentUserId("1");

        // Assert
        currentUser.Id.Should().Be("1");
    }
}
