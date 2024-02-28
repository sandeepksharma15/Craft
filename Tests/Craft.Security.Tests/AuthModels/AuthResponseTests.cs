using Craft.Security.AuthModels;
using FluentAssertions;
using Moq;
using Xunit;

namespace Craft.Security.Tests.AuthModels;

public class AuthResponseTests
{
    [Fact]
    public void Constructor_SetsPropertiesCorrectly()
    {
        // Arrange
        const string jwtToken = "test-jwt-token";
        const string refreshToken = "test-refresh-token";
        var expiryTime = DateTime.UtcNow.AddHours(1);

        // Act
        var response = new AuthResponse(jwtToken, refreshToken, expiryTime);

        // Assert
        response.JwtToken.Should().Be(jwtToken);
        response.RefreshToken.Should().Be(refreshToken);
        response.RefreshTokenExpiryTime.Should().Be(expiryTime);
    }

    [Fact]
    public void Empty_ReturnsEmptyResponse()
    {
        // Act
        var emptyResponse = AuthResponse.Empty;

        // Assert
        emptyResponse.IsEmpty.Should().BeTrue();
        emptyResponse.JwtToken.Should().BeEmpty();
        emptyResponse.RefreshToken.Should().BeEmpty();
        emptyResponse.RefreshTokenExpiryTime.Should().Be(DateTime.MinValue);
    }

    [Fact]
    public void IsEmpty_ReturnsTrueForEmptyValues()
    {
        // Arrange
        var response = new AuthResponse("", "", DateTime.MinValue);

        // Act & Assert
        response.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void IsEmpty_ReturnsFalseForNonEmptyValues()
    {
        // Arrange
        var response = new AuthResponse("token", "refresh", DateTime.UtcNow.AddHours(1));

        // Act & Assert
        response.IsEmpty.Should().BeFalse();
    }

    [Fact]
    public void GetAuthResult_EmptyJson_ReturnsEmpty()
    {
        // Arrange
        const string emptyJson = "";

        // Act
        var result = AuthResponse.GetAuthResult(emptyJson);

        // Assert
        result.Should().Be(AuthResponse.Empty);
    }

    [Fact]
    public void GetAuthResult_ValidJson_DeserializesCorrectly()
    {
        // Arrange
        const string token = "test-token";
        const string refresh = "refresh-token";
        var expiry = DateTime.UtcNow.AddHours(1);
        var json = $"{{\"jwtToken\": \"{token}\", \"refreshToken\": \"{refresh}\", \"refreshTokenExpiryTime\": \"{expiry:yyyy-MM-ddTHH:mm:ssZ}\"}}";

        // Act
        var result = AuthResponse.GetAuthResult(json);

        // Assert
        result.JwtToken.Should().Be(token);
        result.RefreshToken.Should().Be(refresh);
    }

    [Fact]
    public void GetAuthResult_NullObject_ReturnsEmpty()
    {
        // Act
        var result = AuthResponse.GetAuthResult(null);

        // Assert
        result.Should().Be(AuthResponse.Empty);
    }

    [Fact]
    public void ToRefreshTokenRequest_CreatesRequestWithCorrectValues()
    {
        // Arrange
        const string jwtToken = "test-jwt-token";
        const string refreshToken = "test-refresh-token";
        var response = new AuthResponse(jwtToken, refreshToken, DateTime.UtcNow);
        const string ipAddress = "127.0.0.1";

        // Act
        var request = response.ToRefreshTokenRequest(ipAddress);

        // Assert
        request.JwtToken.Should().Be(jwtToken);
        request.RefreshToken.Should().Be(refreshToken);
    }

    [Fact]
    public void GetAuthResult_Object_CallsStringOverload()
    {
        // Arrange
        const string token = "test-token";
        const string refresh = "refresh-token";
        var expiry = DateTime.UtcNow.AddHours(1);
        var json = $"{{\"jwtToken\": \"{token}\", \"refreshToken\": \"{refresh}\", \"refreshTokenExpiryTime\": \"{expiry:yyyy-MM-ddTHH:mm:ssZ}\"}}";

        // Act
        var result = AuthResponse.GetAuthResult((object)json);

        // Assert
        result.JwtToken.Should().Be(token);
        result.RefreshToken.Should().Be(refresh);
    }
}
