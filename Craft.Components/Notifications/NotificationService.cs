using Microsoft.JSInterop;

namespace Craft.Components.Notifications;

internal class NotificationService(IJSRuntime jsRuntime) : INotificationService
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask = new(() =>
            jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Craft.Components/scripts.js").AsTask());

    public PermissionType PermissionStatus { get; private set; }

    public async ValueTask CreateAsync(string title, NotificationOptions options)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("create", title, options);
    }

    public async ValueTask CreateAsync(string title, string description, string iconUrl = null)
    {
        var module = await _moduleTask.Value;

        NotificationOptions options = new()
        {
            Body = description,
            Icon = iconUrl,
        };

        await module.InvokeVoidAsync("create", title, options);
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }

    public async ValueTask<bool> IsSupportedByBrowserAsync()
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<bool>("isSupported");
    }

    public async ValueTask<PermissionType> RequestPermissionAsync()
    {
        var module = await _moduleTask.Value;
        string permission = await module.InvokeAsync<string>("requestPermission");

        if (permission.Equals("granted", StringComparison.InvariantCultureIgnoreCase))
            PermissionStatus = PermissionType.Granted;

        if (permission.Equals("denied", StringComparison.InvariantCultureIgnoreCase))
            PermissionStatus = PermissionType.Denied;

        return PermissionStatus;
    }
}
