using Craft.Utilities.Managers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Craft.Utilities.Tests.Managers;

public class ObserverManagerTests
{
    private Mock<ILogger<ObserverManager<int, string>>> loggerMock;
    private ObserverManager<int, string> observerManager;
    private Mock<IObserver<string>> observerMock;

    public ObserverManagerTests()
    {
        loggerMock = new Mock<ILogger<ObserverManager<int, string>>>();
        observerManager = new ObserverManager<int, string>(loggerMock.Object);
        observerMock = new Mock<IObserver<string>>();
    }

    [Fact]
    public void Subscribe_AddsObserverToObserversDictionary()
    {
        // Arrange
        observerManager.Clear();

        // Act
        observerManager.Subscribe(1, observerMock.Object);

        // Assert
        observerManager.Observers.Should().ContainKey(1);
        observerManager.Observers[1].Should().Be(observerMock.Object);
    }

    [Fact]
    public void Subscribe_UpdatesObserverIfAlreadyExists()
    {
        // Arrange
        observerManager.Clear();
        var observerMock1 = new Mock<IObserver<string>>();
        var observerMock2 = new Mock<IObserver<string>>();
        observerManager.Subscribe(1, observerMock1.Object);

        // Act
        observerManager.Subscribe(1, observerMock2.Object);

        // Assert
        observerManager.Observers.Should().ContainKey(1);
        observerManager.Observers[1].Should().Be(observerMock2.Object);
    }

    [Fact]
    public void Unsubscribe_RemovesObserverFromObserversDictionary()
    {
        // Arrange
        observerManager.Clear();
        observerManager.Subscribe(1, observerMock.Object);

        // Act
        observerManager.Unsubscribe(1);

        // Assert
        observerManager.Observers.Should().NotContainKey(1);
    }

    [Fact]
    public void Notify_CallsOnNextForAllObservers()
    {
        // Arrange
        observerManager.Clear();
        var observerMock1 = new Mock<IObserver<string>>();
        var observerMock2 = new Mock<IObserver<string>>();
        observerManager.Subscribe(1, observerMock1.Object);
        observerManager.Subscribe(2, observerMock2.Object);

        // Act
        observerManager.Notify("test");

        // Assert
        observerMock1.Verify(x => x.OnNext("test"), Times.Once);
        observerMock2.Verify(x => x.OnNext("test"), Times.Once);
    }

    [Fact]
    public void Notify_RemovesErroredObservers()
    {
        // Arrange
        observerManager.Clear();
        var observerMock1 = new Mock<IObserver<string>>();
        var observerMock2 = new Mock<IObserver<string>>();
        observerManager.Subscribe(1, observerMock1.Object);
        observerManager.Subscribe(2, observerMock2.Object);

        // Setup an observer that throws an exception
        observerMock1.Setup(x => x.OnNext(It.IsAny<string>())).Throws<Exception>();

        // Act
        observerManager.Notify("test");

        // Assert
        observerManager.Observers.Should().NotContainKey(1);
        observerMock1.Verify(x => x.OnNext("test"), Times.Once);
        observerMock2.Verify(x => x.OnNext("test"), Times.Once);
    }

    [Fact]
    public void Count_ReturnsCorrectNumberOfObservers()
    {
        // Arrange
        observerManager.Clear();
        var observerMock1 = new Mock<IObserver<string>>();
        var observerMock2 = new Mock<IObserver<string>>();
        observerManager.Subscribe(1, observerMock1.Object);
        observerManager.Subscribe(2, observerMock2.Object);

        // Act
        var count = observerManager.Count;

        // Assert
        count.Should().Be(2);
    }

    [Fact]
    public void Clear_RemovesAllObservers()
    {
        // Arrange
        observerManager.Clear();
        var observerMock1 = new Mock<IObserver<string>>();
        var observerMock2 = new Mock<IObserver<string>>();
        observerManager.Subscribe(1, observerMock1.Object);
        observerManager.Subscribe(2, observerMock2.Object);

        // Act
        observerManager.Clear();

        // Assert
        observerManager.Observers.Should().BeEmpty();
    }

    [Fact]
    public void GetEnumerator_ReturnsAllObservers()
    {
        // Arrange
        observerManager.Clear();
        var observerMock1 = new Mock<IObserver<string>>();
        var observerMock2 = new Mock<IObserver<string>>();
        observerManager.Subscribe(1, observerMock1.Object);
        observerManager.Subscribe(2, observerMock2.Object);

        // Act
        var observers = observerManager.ToList();

        // Assert
        observers.Should().ContainInOrder(observerMock1.Object, observerMock2.Object);
    }
}
