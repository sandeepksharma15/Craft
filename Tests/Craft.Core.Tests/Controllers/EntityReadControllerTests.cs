using Craft.Core.Controllers;
using Craft.Domain.Repositories;
using Craft.TestHelper.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Craft.Core.Tests.Controllers;

public class EntityReadControllerTests
{
    public class TestController<T, DataTransferT>(IRepository<TestEntity> repository,
        ILogger<TestController<T, DataTransferT>> logger)
            : EntityReadController<TestEntity, TestDto>(repository, logger) { }

    private readonly Mock<ILogger<TestController<TestEntity, TestDto>>> _loggerMock;
    private readonly Mock<IRepository<TestEntity>> _repositoryMock;

    public EntityReadControllerTests()
    {
        _repositoryMock = new Mock<IRepository<TestEntity>>();
        _loggerMock = new Mock<ILogger<TestController<TestEntity, TestDto>>>();
    }

    [Fact]
    public async Task GetAllAsync_WithIncludeDetailsTrue_ReturnsOk()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.GetAllAsync(true, default))
            .ReturnsAsync([]);
        var controller = new TestController<TestEntity, TestDto>(_repositoryMock.Object, _loggerMock.Object);

        // Act
        var result = await controller.GetAllAsync(true);
        var okResult = result.Result as OkObjectResult;

        // Assert
        result.Should().BeOfType<ActionResult<IAsyncEnumerable<TestEntity>>>();
        result.Result.Should().BeOfType<OkObjectResult>();
        okResult.Value.Should().BeOfType<List<TestEntity>>();
    }

    [Fact]
    public async Task GetAsync_WithInvalidId_ReturnsNotFoundResult()
    {
        // Arrange
        const long id = 1L;
        const bool includeDetails = true;

        _repositoryMock
            .Setup(repo => repo.GetAsync(id, includeDetails, CancellationToken.None))
            .ReturnsAsync((TestEntity)null);
        var controller = new TestController<TestEntity, TestDto>(_repositoryMock.Object, _loggerMock.Object);

        // Act
        var result = await controller.GetAsync(id, includeDetails);

        // Assert
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetAsync_WithValidIdAndIncludeDetailsTrue_ReturnsOkResult()
    {
        // Arrange
        const long id = 1L;
        const bool includeDetails = true;
        var expectedEntity = new TestEntity { Id = id };

        _repositoryMock
            .Setup(repo => repo.GetAsync(id, includeDetails, CancellationToken.None))
            .ReturnsAsync(expectedEntity);
        var controller = new TestController<TestEntity, TestDto>(_repositoryMock.Object, _loggerMock.Object);

        // Act
        var result = await controller.GetAsync(id, includeDetails);

        // Assert
        result.Should().BeOfType<ActionResult<TestEntity>>();
        result.Result.Should().BeOfType<OkObjectResult>();
        result.Result.As<OkObjectResult>().Value.Should().Be(expectedEntity);
    }

    [Fact]
    public async Task GetCountAsync_WithException_ReturnsProblem()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.GetCountAsync(CancellationToken.None))
            .Throws(new Exception("Test Exception"));
        var controller = new TestController<TestEntity, TestDto>(_repositoryMock.Object, _loggerMock.Object);

        // Act
        var result = await controller.GetCountAsync();
        var value = result.Result as ObjectResult;
        // Assert
        result.Result.Should().BeOfType<ObjectResult>();
        value.Value.Should().BeOfType<ProblemDetails>();
    }
}
