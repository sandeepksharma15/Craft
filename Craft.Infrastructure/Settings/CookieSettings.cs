namespace Craft.Infrastructure.Settings;

public class CookieSettings
{
    public string AccessDeniedPath { get; set; }
    public int ExpireTimeSpan { get; set; }
    public bool HttpOnly { get; set; }
    public string LoginPath { get; set; }
    public string LogoutPath { get; set; }
    public bool SlidingExpiration { get; set; }
}
