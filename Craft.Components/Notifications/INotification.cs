namespace Craft.Components.Notifications;

public interface INotification
{
    string Badge { get; }
    string Body { get; }
    object Data { get; }
    string Dir { get; }
    string Icon { get; }
    Guid Id { get; set; }
    string Image { get; set; }
    string Language { get; }
    bool? NoScreen { get; }
    bool? ReNotify { get; }
    bool? RequireInteraction { get; }
    bool? Silent { get; }
    string Sound { get; }
    bool? Sticky { get; }
    string Tag { get; }
    int TimeOut { get; }
    DateTime? TimeStamp { get; }
    string Title { get; }
}
