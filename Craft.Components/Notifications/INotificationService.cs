namespace Craft.Components.Notifications;

public interface INotificationService
{
    PermissionType PermissionStatus { get; }

    ValueTask CreateAsync(string title, NotificationOptions options);

    ValueTask CreateAsync(string title, string description, string iconUrl = null);

    ValueTask<bool> IsSupportedByBrowserAsync();

    ValueTask<PermissionType> RequestPermissionAsync();
}
