using Craft.Components.Notifications;
using FluentAssertions;
using Mapster;
using Tynamix.ObjectFiller;
using Xunit;

namespace Craft.Components.Tests.Notifications;

public class NotificationTests
{
    [Fact]
    public void Notification_CreatedWithDefaultValues_ShouldHaveValidProperties()
    {
        // Arrange
        const string title = "Test Notification";

        // Act
        var notification = new Notification(title);

        // Assert
        notification.Id.Should().NotBeEmpty();
        notification.Title.Should().Be(title);
        notification.TimeStamp.Should().NotBeNull();
        notification.Dir.Should().BeNull();
        notification.Language.Should().BeNull();
        notification.Badge.Should().BeNull();
        notification.Body.Should().BeNull();
        notification.Tag.Should().BeNull();
        notification.Icon.Should().BeNull();
        notification.Image.Should().BeNull();
        notification.Data.Should().BeNull();
        notification.ReNotify.Should().BeNull();
        notification.RequireInteraction.Should().BeNull();
        notification.Silent.Should().BeNull();
        notification.Sound.Should().BeNull();
        notification.NoScreen.Should().BeNull();
        notification.Sticky.Should().BeNull();
        notification.TimeOut.Should().Be(5);
    }

    [Fact]
    public void Notification_CreatedWithOptions_ShouldApplyOptions()
    {
        // Arrange
        const string title = "Test Notification";
        var options = new NotificationOptions
        {
            Image = "image.jpg",
            TimeOut = 10
        };

        // Act
        var notification = new Notification(title, options);

        // Assert
        notification.Image.Should().Be(options.Image);
        notification.TimeOut.Should().Be(options.TimeOut);
    }

    [Fact]
    public void Notification_MappingFrom_NotificationOptions_ShouldWork()
    {
        // Arrange
        Filler<NotificationOptions> objectFiller = new();

        objectFiller.Setup()
            .OnType<object>().Use(() => null);

        NotificationOptions options = objectFiller.Create();

        // Act
        var notification = options.Adapt<Notification>();

        // Assert - Compare all properties
        notification.TimeStamp.Should().Be(options.TimeStamp);
        notification.Dir.Should().Be(options.Dir);
        notification.Language.Should().Be(options.Language);
        notification.Badge.Should().Be(options.Badge);
        notification.Body.Should().Be(options.Body);
        notification.Tag.Should().Be(options.Tag);
        notification.Icon.Should().Be(options.Icon);
        notification.Image.Should().Be(options.Image);
        notification.Data.Should().Be(options.Data);
        notification.ReNotify.Should().Be(options.ReNotify);
        notification.RequireInteraction.Should().Be(options.RequireInteraction);
        notification.Silent.Should().Be(options.Silent);
        notification.Sound.Should().Be(options.Sound);
        notification.NoScreen.Should().Be(options.NoScreen);
        notification.Sticky.Should().Be(options.Sticky);
        notification.TimeOut.Should().Be(options.TimeOut);
    }

    [Fact]
    public void Notification_TitleIsEmpty_ShouldThrowException()
    {
        // Arrange
        const string title = "";

        // Act
        var action = new Action(() => new Notification(title));

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Notification_TitleIsNull_ShouldThrowException()
    {
        // Arrange
        const string title = null;

        // Act
        Action action = new(() => new Notification(title));

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }
}
