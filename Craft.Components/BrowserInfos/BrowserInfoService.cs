using Microsoft.JSInterop;

namespace Craft.Components.BrowserInfos;

internal class BrowserInfoService(IJSRuntime jsRuntime) : IBrowserInfoService
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask = new(() =>
            jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Craft.Components/browserDetect.js").AsTask());

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }

    public async ValueTask<BrowserInfo> GetBrowserInfoAsync()
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<BrowserInfo>("browserDetect");
    }
}
