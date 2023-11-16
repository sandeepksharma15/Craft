using Craft.Utilities.Managers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Craft.Utilities.Tests.Managers;

public class ObserverManagerTests
{
    private readonly Mock<ILogger<ObserverManager<int>>> loggerMock;

    public ObserverManagerTests()
    {
        loggerMock = new Mock<ILogger<ObserverManager<int>>>();
    }

    [Fact]
    public void Subscribe_AddsObserverToList()
    {
        // Arrange
        var observerManager = new ObserverManager<int>(loggerMock.Object);
        var observerMock = new Mock<IObserver<int>>();

        // Act
        observerManager.Subscribe(observerMock.Object);

        // Assert
        observerManager.Observers.Should().Contain(observerMock.Object);
    }

    [Fact]
    public void Subscribe_DoesNotAddDuplicateObserver()
    {
        // Arrange
        var observerManager = new ObserverManager<int>(loggerMock.Object);
        var observerMock = new Mock<IObserver<int>>();
        observerManager.Subscribe(observerMock.Object);

        // Act
        observerManager.Subscribe(observerMock.Object);

        // Assert
        observerManager.Observers.Should().HaveCount(1);
    }

    [Fact]
    public void Unsubscribe_RemovesObserverFromList()
    {
        // Arrange
        var observerManager = new ObserverManager<int>(loggerMock.Object);
        var observerMock = new Mock<IObserver<int>>();
        observerManager.Subscribe(observerMock.Object);

        // Act
        observerManager.Unsubscribe(observerMock.Object);

        // Assert
        observerManager.Observers.Should().NotContain(observerMock.Object);
    }

    [Fact]
    public void Notify_CallsOnNextOnObservers()
    {
        // Arrange
        var observerManager = new ObserverManager<int>(loggerMock.Object);
        var observerMock1 = new Mock<IObserver<int>>();
        var observerMock2 = new Mock<IObserver<int>>();
        observerManager.Subscribe(observerMock1.Object);
        observerManager.Subscribe(observerMock2.Object);

        // Act
        observerManager.Notify(42);

        // Assert
        observerMock1.Verify(observer => observer.OnNext(42), Times.Once);
        observerMock2.Verify(observer => observer.OnNext(42), Times.Once);
    }

    [Fact]
    public void Notify_HandlesExceptionAndRemovesDefunctObservers()
    {
        // Arrange
        var observerManager = new ObserverManager<int>(loggerMock.Object);
        var observerMock1 = new Mock<IObserver<int>>();
        var observerMock2 = new Mock<IObserver<int>>();
        observerManager.Subscribe(observerMock1.Object);
        observerManager.Subscribe(observerMock2.Object);
        observerMock2.Setup(observer => observer.OnNext(It.IsAny<int>())).Throws<Exception>();

        // Act
        observerManager.Notify(42);

        // Assert
        observerMock1.Verify(observer => observer.OnNext(42), Times.Once);
        observerManager.Observers.Should().NotContain(observerMock2.Object);
    }

    [Fact]
    public void Count_ReturnsCorrectNumberOfObservers()
    {
        // Arrange
        var observerManager = new ObserverManager<int>(loggerMock.Object);
        var observerMock1 = new Mock<IObserver<int>>();
        var observerMock2 = new Mock<IObserver<int>>();
        observerManager.Subscribe(observerMock1.Object);
        observerManager.Subscribe(observerMock2.Object);

        // Act
        var count = observerManager.Count;

        // Assert
        count.Should().Be(2);
    }

    [Fact]
    public void Clear_RemovesAllObservers()
    {
        // Arrange
        var observerManager = new ObserverManager<int>(loggerMock.Object);
        var observerMock1 = new Mock<IObserver<int>>();
        var observerMock2 = new Mock<IObserver<int>>();
        observerManager.Subscribe(observerMock1.Object);
        observerManager.Subscribe(observerMock2.Object);

        // Act
        observerManager.Clear();

        // Assert
        observerManager.Observers.Should().BeEmpty();
    }

    [Fact]
    public void GetEnumerator_ReturnsAllObservers()
    {
        // Arrange
        var observerManager = new ObserverManager<int>(loggerMock.Object);
        var observerMock1 = new Mock<IObserver<int>>();
        var observerMock2 = new Mock<IObserver<int>>();
        observerManager.Subscribe(observerMock1.Object);
        observerManager.Subscribe(observerMock2.Object);

        // Act
        var observers = observerManager.ToList();

        // Assert
        observers.Should().ContainInOrder(observerMock1.Object, observerMock2.Object);
    }
}
