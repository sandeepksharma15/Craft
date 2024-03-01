namespace Craft.Components.BrowserInfos;

public interface IBrowserInfoService
{
    ValueTask<BrowserInfo> GetBrowserInfoAsync();
}
