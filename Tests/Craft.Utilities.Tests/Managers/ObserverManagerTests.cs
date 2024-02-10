using Craft.Utilities.Managers;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Craft.Utilities.Tests.Managers;

public class ObserverManagerTests
{
    private readonly Mock<ILogger<ObserverManager<int, string>>> loggerMock;
    private readonly ObserverManager<int, string> observerManager;

    public ObserverManagerTests()
    {
        loggerMock = new Mock<ILogger<ObserverManager<int, string>>>();
        observerManager = new ObserverManager<int, string>(loggerMock.Object);
    }

    [Fact]
    public void Clear_RemovesAllObservers()
    {
        // Arrange
        observerManager.Clear();
        observerManager.Subscribe(1, "Observer1");
        observerManager.Subscribe(2, "Observer2");
        observerManager.Subscribe(3, "Observer3");

        // Act
        observerManager.Clear();

        // Assert
        observerManager.Observers.Should().BeEmpty();
    }

    [Fact]
    public void Count_ReturnsCorrectNumberOfObservers()
    {
        // Arrange
        observerManager.Clear();
        observerManager.Clear();
        observerManager.Subscribe(1, "Observer1");
        observerManager.Subscribe(2, "Observer2");
        observerManager.Subscribe(3, "Observer3");

        // Act
        var count = observerManager.Count;

        // Assert
        count.Should().Be(3);
    }

    [Fact]
    public void GetEnumerator_ReturnsAllObservers()
    {
        // Arrange
        observerManager.Clear();
        observerManager.Subscribe(1, "Observer1");
        observerManager.Subscribe(2, "Observer2");
        observerManager.Subscribe(3, "Observer3");

        // Act
        var observers = observerManager.ToList();

        // Assert
        observers.Should().ContainInOrder("Observer1", "Observer2", "Observer3");
    }

    [Fact]
    public async Task Notify_RemovesErroredObservers()
    {
        // Arrange
        observerManager.Clear();
        const string observer1 = "Observer1";
        const string observer2 = "Observer2";
        const string observer3 = "Observer3";

        observerManager.Subscribe(1, observer1);
        observerManager.Subscribe(2, observer2);
        observerManager.Subscribe(3, observer3);

        // Act
        static Task NotificationAsync(string observer)
        {
            if (observer == observer2)
                throw new Exception("Notification failed");

            return Task.CompletedTask;
        }

        await observerManager.NotifyAsync(NotificationAsync);

        // Assert
        observerManager.Observers.Should().NotContainKey(2);
        observerManager.Observers.Should().ContainKey(1);
        observerManager.Observers.Should().ContainKey(3);
    }

    [Fact]
    public async Task NotifyAsync_NotifiesAllObservers()
    {
        // Arrange
        var notificationCalledCount = 0;
        observerManager.Clear();
        observerManager.Subscribe(1, "Observer1");
        observerManager.Subscribe(2, "Observer2");

        // Act
        async Task NotificationAsync(string _)
        {
            notificationCalledCount++;
            await Task.CompletedTask;
        }

        await observerManager.NotifyAsync(NotificationAsync);

        // Assert
        notificationCalledCount.Should().Be(2);
    }

    [Fact]
    public void Subscribe_AddsNewObserver()
    {
        // Arrange
        observerManager.Clear();

        // Act
        observerManager.Subscribe(1, "Observer1");

        // Assert
        observerManager.Count.Should().Be(1);
        observerManager.Observers.Should().ContainKey(1).And.ContainValue("Observer1");
    }

    [Fact]
    public void Subscribe_UpdatesExistingObserver()
    {
        // Arrange
        observerManager.Clear();
        observerManager.Subscribe(1, "Observer1");

        // Act
        observerManager.Subscribe(1, "Observer2");

        // Assert
        observerManager.Count.Should().Be(1);
        observerManager.Observers.Should().ContainKey(1).And.ContainValue("Observer2");
    }

    [Fact]
    public void Unsubscribe_RemovesObserver()
    {
        // Arrange
        observerManager.Clear();
        observerManager.Subscribe(1, "Observer1");

        // Act
        observerManager.Unsubscribe(1);

        // Assert
        observerManager.Count.Should().Be(0);
        observerManager.Observers.Should().NotContainKey(1);
    }
}
