using Mapster;

namespace Craft.Components.Notifications;

public class Notification : INotification
{
    public string Badge { get; set; }
    public string Body { get; set; }
    public object Data { get; set; }
    public string Dir { get; set; }
    public string Icon { get; set; }
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Image { get; set; }
    public string Language { get; set; }
    public bool? NoScreen { get; set; }
    public bool? ReNotify { get; set; }
    public bool? RequireInteraction { get; set; }
    public bool? Silent { get; set; }
    public string Sound { get; set; }
    public bool? Sticky { get; set; }
    public string Tag { get; set; }
    public int TimeOut { get; set; } = 5;
    public DateTime? TimeStamp { get; set; } = DateTime.UtcNow;
    public string Title { get; set; }

    public Notification(string title, NotificationOptions options = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));

        Title = title;

        options?.Adapt(this);
    }

    // Removing this constructor causes the error with "Mapster"
    public Notification() { }
}
