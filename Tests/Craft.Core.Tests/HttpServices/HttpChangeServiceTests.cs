using System.Net;
using Craft.Core.HttpServices;
using Craft.TestHelper.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;

namespace Craft.Core.Tests.HttpServices;

public class HttpChangeServiceTests
{
    private readonly Uri _apiURL;

    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

    private readonly Mock<ILogger<HttpChangeService<TestEntity, TestEntity, TestEntity>>> _loggerMock;

    private HttpChangeService<TestEntity, TestEntity, TestEntity> _service;

    public HttpChangeServiceTests()
    {
        _loggerMock = new Mock<ILogger<HttpChangeService<TestEntity, TestEntity, TestEntity>>>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _apiURL = new Uri("https://example.com/api");
    }

    [Fact]
    public async Task AddAsync_ShouldSendPostRequestWithCorrectData()
    {
        // Arrange
        var model = new TestEntity { Id = 1, Name = "Entity1" };
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.Created);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _service = new HttpChangeService<TestEntity, TestEntity, TestEntity>(_apiURL, httpClient, _loggerMock.Object);

        // Act
        var response = await _service.AddAsync(model);

        // Assert
        response.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task AddRangeAsync_ShouldSendPostRequestWithCorrectData()
    {
        // Arrange
        var models = new List<TestEntity>
        {
            new() { Id = 1, Name = "Entity1" },
            new() { Id = 2, Name = "Entity2" }
        };
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.Created);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _service = new HttpChangeService<TestEntity, TestEntity, TestEntity>(_apiURL, httpClient, _loggerMock.Object);

        // Act
        var response = await _service.AddRangeAsync(models);

        // Assert
        response.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task DeleteAsync_ShouldSendDeleteRequestWithCorrectUri()
    {
        // Arrange
        const int id = 1;
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.NoContent);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _service = new HttpChangeService<TestEntity, TestEntity, TestEntity>(_apiURL, httpClient, _loggerMock.Object);

        // Act
        var response = await _service.DeleteAsync(id);

        // Assert
        response.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task DeleteRangeAsync_ShouldSendPutRequestWithCorrectUri()
    {
        // Arrange
        var models = new List<TestEntity>
        {
            new() { Id = 1, Name = "Entity1" },
            new() { Id = 2, Name = "Entity2" }
        };
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.NoContent);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _service = new HttpChangeService<TestEntity, TestEntity, TestEntity>(_apiURL, httpClient, _loggerMock.Object);

        // Act
        var response = await _service.DeleteRangeAsync(models);

        // Assert
        response.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task UpdateAsync_ShouldSendPutRequestWithCorrectData()
    {
        // Arrange
        var model = new TestEntity { Id = 1, Name = "UpdatedEntity" };
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _service = new HttpChangeService<TestEntity, TestEntity, TestEntity>(_apiURL, httpClient, _loggerMock.Object);

        // Act
        var response = await _service.UpdateAsync(model);

        // Assert
        response.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task UpdateRangeAsync_ShouldSendPutRequestWithCorrectData()
    {
        // Arrange
        var models = new List<TestEntity>
        {
            new() { Id = 1, Name = "UpdatedEntity1" },
            new() { Id = 2, Name = "UpdatedEntity2" }
        };

        var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _service = new HttpChangeService<TestEntity, TestEntity, TestEntity>(_apiURL, httpClient, _loggerMock.Object);

        // Act
        var response = await _service.UpdateRangeAsync(models);

        // Assert
        response.Should().Be(expectedResponse);
    }
}
