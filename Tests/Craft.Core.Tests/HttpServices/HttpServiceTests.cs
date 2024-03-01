using System.Net;
using Craft.Core.HttpServices;
using Craft.Domain.Helpers;
using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Core;
using Craft.TestHelper.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Xunit;

namespace Craft.Core.Tests.HttpServices;

public class HttpServiceTests
{
    private readonly Uri _apiURL;
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly Mock<ILogger<HttpService<TestEntity, TestEntity, TestEntity>>> _loggerMock;
    private HttpService<TestEntity, TestEntity, TestEntity> _service;
    private readonly IQuery<TestEntity> query = new Query<TestEntity>();
    private readonly IQuery<TestEntity, CompanyName> nameQuery = new Query<TestEntity, CompanyName>();

    public HttpServiceTests()
    {
        _loggerMock = new Mock<ILogger<HttpService<TestEntity, TestEntity, TestEntity>>>();
        _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
        _apiURL = new Uri("https://example.com/api");
    }

    [Fact]
    public async Task DeleteAsync_ShouldSendPostRequestWithCorrectData()
    {
        // Arrange
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.NoContent);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _service = new HttpService<TestEntity, TestEntity, TestEntity>(_apiURL, httpClient, _loggerMock.Object);

        // Act
        var response = await _service.DeleteAsync(query);

        // Assert
        response.Should().Be(expectedResponse);
    }

    [Fact]
    public async Task GetAsync_ShouldSendPostRequestWithCorrectData()
    {
        // Arrange
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);
        var entity = new TestEntity { Id = 1, Name = "Entity1" };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        expectedResponse.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(entity));
        expectedResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _service = new HttpService<TestEntity, TestEntity, TestEntity>(_apiURL, httpClient, _loggerMock.Object);

        // Act
        var response = await _service.GetAsync(query);

        // Assert
        response.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public async Task GetAsyncResult_ShouldSendPostRequestWithCorrectData()
    {
        // Arrange
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);
        var entity = new CompanyName { Name = "Entity1" };

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        expectedResponse.Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(entity));
        expectedResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _service = new HttpService<TestEntity, TestEntity, TestEntity>(_apiURL, httpClient, _loggerMock.Object);

        // Act
        var response = await _service.GetAsync(nameQuery);

        // Assert
        response.Should().BeEquivalentTo(entity);
    }

    [Fact]
    public async Task GetCountAsync_ShouldSendPostRequestWithCorrectData()
    {
        // Arrange
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);
        const int count = 10;

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        expectedResponse.Content = new StringContent(count.ToString());
        expectedResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _service = new HttpService<TestEntity, TestEntity, TestEntity>(_apiURL, httpClient, _loggerMock.Object);

        // Act
        var response = await _service.GetCountAsync(query);

        // Assert
        response.Should().Be(count);
    }

    [Fact]
    public async Task GetPagedListAsync_ShouldSendPostRequestWithCorrectData()
    {
        // Arrange
        const int page = 1;
        const int pageSize = 10;
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);
        var entities = new List<TestEntity>
        {
            new() { Id = 1, Name = "Entity1" },
            new() { Id = 2, Name = "Entity2" }
        };
        var totalCount = entities.Count;

        var pageResponse = new PageResponse<TestEntity>(entities, totalCount, page, pageSize);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(pageResponse);

        expectedResponse.Content = new StringContent(jsonData);
        expectedResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _service = new HttpService<TestEntity, TestEntity, TestEntity>(_apiURL, httpClient, _loggerMock.Object);

        // Act
        var response = await _service.GetPagedListAsync(query);

        // Assert
        response.Items.Should().BeEquivalentTo(entities);
        response.TotalCount.Should().Be(totalCount);
        response.CurrentPage.Should().Be(page);
        response.PageSize.Should().Be(pageSize);
        response.TotalPages.Should().Be(1);
        response.From.Should().Be(1);
        response.To.Should().Be(totalCount);
        response.HasPreviousPage.Should().BeFalse();
        response.HasNextPage.Should().BeFalse();
    }

    [Fact]
    public async Task GetPagedListResultAsync_ShouldSendPostRequestWithCorrectData()
    {
        // Arrange
        const int page = 1;
        const int pageSize = 10;
        var expectedResponse = new HttpResponseMessage(HttpStatusCode.OK);
        var entities = new List<CompanyName>
        {
            new() { Name = "Entity1" },
            new() { Name = "Entity2" }
        };
        var totalCount = entities.Count;

        var pageResponse = new PageResponse<CompanyName>(entities, totalCount, page, pageSize);

        _httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

        var jsonData = Newtonsoft.Json.JsonConvert.SerializeObject(pageResponse);

        expectedResponse.Content = new StringContent(jsonData);
        expectedResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        var httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        _service = new HttpService<TestEntity, TestEntity, TestEntity>(_apiURL, httpClient, _loggerMock.Object);

        // Act
        var response = await _service.GetPagedListAsync(nameQuery);

        // Assert
        response.Items.Should().BeEquivalentTo(entities);
        response.TotalCount.Should().Be(totalCount);
        response.CurrentPage.Should().Be(page);
        response.PageSize.Should().Be(pageSize);
        response.TotalPages.Should().Be(1);
        response.From.Should().Be(1);
        response.To.Should().Be(totalCount);
        response.HasPreviousPage.Should().BeFalse();
        response.HasNextPage.Should().BeFalse();
    }
}
