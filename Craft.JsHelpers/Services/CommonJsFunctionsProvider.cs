using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Craft.JsHelpers.Services;

public class CommonJsFunctionsProvider : IAsyncDisposable
{
    private readonly ILogger<CommonJsFunctionsProvider> _logger;
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0290:Use primary constructor", Justification = "<Pending>")]
    public CommonJsFunctionsProvider(IJSRuntime jsRuntime, ILogger<CommonJsFunctionsProvider> logger)
    {
        _logger = logger;

        _moduleTask = new(() => jsRuntime
            .InvokeAsync<IJSObjectReference>("import", "./_content/Craft.JsHelpers/commonMethods.js")
            .AsTask());
    }

    public async Task CopyToClipboard(string text)
    {
        _logger.LogDebug("[CommonFunctionsProvider] CopyToClipboard Invoked");

        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("copyToClipboard", text);
    }

    public async Task ChangeCssById(string elementId, string css)
    {
        _logger.LogDebug("[CommonFunctionsProvider] ChangeCssById Invoked");

        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("changeCssById", elementId, css);
    }

    public async Task ChangeCssBySelector(string selector, string css)
    {
        _logger.LogDebug("[CommonFunctionsProvider] ChangeCssBySelector Invoked");

        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("changeCssBySelector", selector, css);
    }

    public async Task UpdateStyleProperty(string elementId, string propertyName, string value)
    {
        _logger.LogDebug("[CommonFunctionsProvider] UpdateStyleProperty Invoked");

        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("updateStyleProperty", elementId, propertyName, value);
    }

    public async Task UpdateStylePropertyBySelector(string selector, string propertyName, string value)
    {
        _logger.LogDebug("[CommonFunctionsProvider] UpdateStylePropertyBySelector Invoked");

        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("updateStylePropertyBySelector", selector, propertyName, value);
    }

    public async Task ChangeGlobalCssVariable(string name, string value)
    {
        _logger.LogDebug("[CommonFunctionsProvider] ChangeGlobalCssVariable Invoked");

        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("changeGlobalCssVariable", name, value);
    }

    public async Task<bool> IsDarkMode()
    {
        _logger.LogDebug("[CommonFunctionsProvider] IsDarkMode Invoked");

        var module = await _moduleTask.Value;

        return await module.InvokeAsync<bool>("isDarkMode");
    }

    public async Task ScrollToTop()
    {
        _logger.LogDebug("[CommonFunctionsProvider] ScrollToTop Invoked");

        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("scrollToTop");
    }

    public async Task<bool> IsTouchSupported()
    {
        _logger.LogDebug("[CommonFunctionsProvider] IsTouchSupported Invoked");

        var module = await _moduleTask.Value;

        return await module.InvokeAsync<bool>("isTouchSupported");
    }

    public async Task Alert(string message)
    {
        _logger.LogDebug("[CommonFunctionsProvider] Alert Invoked");

        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("jsAlert", message);
    }

    public async Task<bool> Confirm(string message)
    {
        _logger.LogDebug("[CommonFunctionsProvider] Confirm Invoked");

        var module = await _moduleTask.Value;

        return await module.InvokeAsync<bool>("jsConfirm", message);
    }

    public async Task<string> Prompt(string message, string defaultValue)
    {
        _logger.LogDebug("[CommonFunctionsProvider] Prompt Invoked");

        var module = await _moduleTask.Value;

        return await module.InvokeAsync<string>("jsPrompt", message, defaultValue);
    }

    public async Task Log(string message)
    {
        _logger.LogDebug("[CommonFunctionsProvider] Log Invoked");

        var module = await _moduleTask.Value;

        await module.InvokeVoidAsync("jsLog", message);
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogDebug("[CommonFunctionsProvider] DisposeAsync Invoked");

#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception.
        try
        {
            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }

            GC.SuppressFinalize(this);
        }
        catch (Exception) { }
#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception.
    }
}
