using System.Net.Http.Json;
using System.Net;
using Craft.Core.HttpServices;
using Craft.Domain.HttpServices;
using Craft.TestHelper.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;

namespace Craft.Core.Tests.HttpServices;

public class HttpReadServiceTests
{
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly IHttpReadService<TestEntity> _readOnlyHttpService;

    public HttpReadServiceTests()
    {
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
        {
            BaseAddress = new Uri("http://example.com")
        };
        var loggerMock = new Mock<ILogger<HttpReadService<TestEntity>>>();

        _readOnlyHttpService = new HttpReadService<TestEntity>(
            new Uri("http://example.com/api/resource"),
            httpClient,loggerMock.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_Should_ReturnListOfEntities_When_HttpRequestSucceeds()
    {
        // Arrange
        var expectedEntities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Entity1" },
            new() { Id = 2, Name = "Entity2" }
        };

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedEntities)
            });

        // Act
        var result = await _readOnlyHttpService.GetAllAsync();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedEntities);
    }

    [Fact]
    public async Task GetAsync_Should_ReturnEntity_When_HttpRequestSucceeds()
    {
        // Arrange
        var expectedEntity = new TestEntity { Id = 1, Name = "Entity1" };
        const int entityId = 1;

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedEntity)
            });

        // Act
        var result = await _readOnlyHttpService.GetAsync(entityId);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedEntity);
    }

    [Fact]
    public async Task GetCountAsync_Should_ReturnCount_When_HttpRequestSucceeds()
    {
        // Arrange
        const int expectedCount = 42;

        _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(expectedCount)
            });

        // Act
        var result = await _readOnlyHttpService.GetCountAsync();

        // Assert
        result.Should().Be(expectedCount);
    }
}
