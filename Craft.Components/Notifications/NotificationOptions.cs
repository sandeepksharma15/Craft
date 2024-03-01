namespace Craft.Components.Notifications;

public class NotificationOptions
{
    public string Badge { get; set; }
    public string Body { get; set; }
    public object Data { get; set; }
    public string Dir { get; set; } = "auto";
    public string Icon { get; set; }
    public string Image { get; set; }
    public string Language { get; set; } = "en";
    public bool? NoScreen { get; set; }
    public bool? ReNotify { get; set; }
    public bool? RequireInteraction { get; set; }
    public bool? Silent { get; set; }
    public string Sound { get; set; }
    public bool? Sticky { get; set; }
    public string Tag { get; set; }
    public int TimeOut { get; set; } = 5;
    public DateTime? TimeStamp { get; set; } = DateTime.UtcNow;
}
