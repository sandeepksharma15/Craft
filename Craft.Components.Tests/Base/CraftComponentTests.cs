using Bunit;
using Craft.Components.Base;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Craft.Components.Tests.Base;

public class CraftComponentTests : TestContext
{
    public class TestComponent : CraftComponent;

    private readonly Mock<ILoggerFactory> _loggerFactoryMock;
    private readonly Mock<ILogger> _loggerMock;

    public CraftComponentTests()
    {
        _loggerFactoryMock = new Mock<ILoggerFactory>();
        _loggerMock = new Mock<ILogger>();
        _loggerFactoryMock.Setup(x =>
            x.CreateLogger(It.IsAny<string>())).Returns(_loggerMock.Object);
    }

    [Fact]
    public void GetId_Returns_Expected_Id_When_UserAttributes_Is_Null()
    {
        // Arrange
        const string expectedId = "testId";
        var _sut = new TestComponent { Id = expectedId };

        // Act
        var result = _sut.GetId();

        // Assert
        result.Should().Be(expectedId);
    }

    [Fact]
    public void GetId_ReturnsId_FromUserAttributeIfExists()
    {
        /// Arrange
        const string expectedId = "testId";
        var userAttributes = new Dictionary<string, object>
            {
                { "id", expectedId }
            };
        var _sut = new TestComponent { UserAttributes = userAttributes };

        // Act
        var result = _sut.GetId();

        // Assert
        result.Should().Be(expectedId);
    }

    [Fact]
    public void GetId_ReturnsId_WhenUserAttributeIdIsNullOrEmpty()
    {
        // Arrange
        const string expectedId = "testId";
        var userAttributes = new Dictionary<string, object>
            {
                { "id", string.Empty }
            };
        var _sut = new TestComponent { UserAttributes = userAttributes, Id = expectedId };

        // Act
        var result = _sut.GetId();

        // Assert
        result.Should().Be(expectedId);
    }

    [Fact]
    public void OnInitialized_Sets_UniqueId()
    {
        // Arrange
        var cut = RenderComponent<TestComponent>();
        var _sut = cut.Instance;

        // Act
        // _sut.OnInitialized();

        // Assert
        _sut.Id.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void SetParametersAsync_Does_Not_Call_StateHasChanged_When_Visible_Is_Not_Changed()
    {
        // Arrange
        var cut = RenderComponent<TestComponent>();
        var _sut = cut.Instance;

        _sut.Visible = false;

        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Visible, false));

        // Assert
        _sut.Visible.Should().BeFalse();
    }

    [Fact]
    public void SetParametersAsync_Calls_StateHasChanged_When_Visible_Is_Changed()
    {
        // Arrange
        var cut = RenderComponent<TestComponent>();
        var _sut = cut.Instance;

        _sut.Visible = true;

        cut.SetParametersAndRender(parameters => parameters
            .Add(p => p.Visible, false));

        // Assert
        _sut.Visible.Should().BeFalse();
    }
}
