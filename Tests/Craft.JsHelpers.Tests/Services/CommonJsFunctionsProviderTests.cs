using System.Threading.Tasks;
using Craft.JsHelpers.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace Craft.JsHelpers.Tests.Services;

public class CommonJsFunctionsProviderTests : TestContext
{
    [Fact]
    public async Task Alert_ShouldInvoke_Alert_InModule()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CommonJsFunctionsProvider>>();
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.JsHelpers/commonMethods.js" });
        JSInterop.SetupVoid("jsAlert", _ => true);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var provider = new CommonJsFunctionsProvider(JSInterop.JSRuntime, loggerMock.Object);

        // Act
        await provider.Alert("Message");

        // Assert
        JSInterop.VerifyInvoke("jsAlert");
    }

    [Fact]
    public async Task ChangeCssById_ShouldInvoke_ChangeCssById_InModule()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CommonJsFunctionsProvider>>();
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.JsHelpers/commonMethods.js" });
        JSInterop.SetupVoid("changeCssById", _ => true);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var provider = new CommonJsFunctionsProvider(JSInterop.JSRuntime, loggerMock.Object);

        // Act
        await provider.ChangeCssById("TestElement", "TestString");

        // Assert
        JSInterop.VerifyInvoke("changeCssById");
    }

    [Fact]
    public async Task ChangeCssBySelector_ShouldInvoke_ChangeCssBySelector_InModule()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CommonJsFunctionsProvider>>();
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.JsHelpers/commonMethods.js" });
        JSInterop.SetupVoid("changeCssBySelector", _ => true);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var provider = new CommonJsFunctionsProvider(JSInterop.JSRuntime, loggerMock.Object);

        // Act
        await provider.ChangeCssBySelector("TestSelector", "TestString");

        // Assert
        JSInterop.VerifyInvoke("changeCssBySelector");
    }

    [Fact]
    public async Task ChangeGlobalCssVariable_ShouldInvoke_ChangeGlobalCssVariable_InModule()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CommonJsFunctionsProvider>>();
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.JsHelpers/commonMethods.js" });
        JSInterop.SetupVoid("changeGlobalCssVariable", _ => true);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var provider = new CommonJsFunctionsProvider(JSInterop.JSRuntime, loggerMock.Object);

        // Act
        await provider.ChangeGlobalCssVariable("Name", "Value");

        // Assert
        JSInterop.VerifyInvoke("changeGlobalCssVariable");
    }

    [Fact]
    public async Task Confirm_ShouldInvoke_Confirm_InModule()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CommonJsFunctionsProvider>>();
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.JsHelpers/commonMethods.js" });
        JSInterop.Setup<bool>("jsConfirm", _ => true).SetResult(true);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var provider = new CommonJsFunctionsProvider(JSInterop.JSRuntime, loggerMock.Object);

        // Act
        await provider.Confirm("Message");

        // Assert
        JSInterop.VerifyInvoke("jsConfirm");
    }

    [Fact]
    public async Task CopyToClipboard_ShouldInvoke_CopyToClipboard_InModule()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CommonJsFunctionsProvider>>();
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.JsHelpers/commonMethods.js" });
        JSInterop.SetupVoid("copyToClipboard", _ => true);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var provider = new CommonJsFunctionsProvider(JSInterop.JSRuntime, loggerMock.Object);

        // Act
        await provider.CopyToClipboard("TestString");

        // Assert
        JSInterop.VerifyInvoke("copyToClipboard");
    }

    [Fact]
    public async Task IsDarkMode_ShouldInvoke_IsDarkMode_InModule()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CommonJsFunctionsProvider>>();
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.JsHelpers/commonMethods.js" });
        JSInterop.Setup<bool>("IsDarkMode", _ => true).SetResult(true);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var provider = new CommonJsFunctionsProvider(JSInterop.JSRuntime, loggerMock.Object);

        // Act
        await provider.IsDarkMode();

        // Assert
        JSInterop.VerifyInvoke("isDarkMode");
    }

    [Fact]
    public async Task IsTouchSupported_ShouldInvoke_IsTouchSupported_InModule()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CommonJsFunctionsProvider>>();
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.JsHelpers/commonMethods.js" });
        JSInterop.Setup<bool>("IsTouchSupported", _ => true).SetResult(true);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var provider = new CommonJsFunctionsProvider(JSInterop.JSRuntime, loggerMock.Object);

        // Act
        await provider.IsTouchSupported();

        // Assert
        JSInterop.VerifyInvoke("isTouchSupported");
    }

    [Fact]
    public async Task Log_ShouldInvoke_Log_InModule()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CommonJsFunctionsProvider>>();
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.JsHelpers/commonMethods.js" });
        JSInterop.SetupVoid("jsLog", _ => true);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var provider = new CommonJsFunctionsProvider(JSInterop.JSRuntime, loggerMock.Object);

        // Act
        await provider.Log("Message");

        // Assert
        JSInterop.VerifyInvoke("jsLog");
    }

    [Fact]
    public async Task Prompt_ShouldInvoke_Prompt_InModule()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CommonJsFunctionsProvider>>();
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.JsHelpers/commonMethods.js" });
        JSInterop.Setup<string>("jsPrompt", _ => true).SetResult("Result");
        JSInterop.Mode = JSRuntimeMode.Loose;

        var provider = new CommonJsFunctionsProvider(JSInterop.JSRuntime, loggerMock.Object);

        // Act
        await provider.Prompt("Message", string.Empty);

        // Assert
        JSInterop.VerifyInvoke("jsPrompt");
    }

    [Fact]
    public async Task ScrollToTop_ShouldInvoke_ScrollToTop_InModule()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CommonJsFunctionsProvider>>();
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.JsHelpers/commonMethods.js" });
        JSInterop.SetupVoid("scrollToTop", _ => true);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var provider = new CommonJsFunctionsProvider(JSInterop.JSRuntime, loggerMock.Object);

        // Act
        await provider.ScrollToTop();

        // Assert
        JSInterop.VerifyInvoke("scrollToTop");
    }

    [Fact]
    public async Task UpdateStyleProperty_ShouldInvoke_UpdateStyleProperty_InModule()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CommonJsFunctionsProvider>>();
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.JsHelpers/commonMethods.js" });
        JSInterop.SetupVoid("updateStyleProperty", _ => true);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var provider = new CommonJsFunctionsProvider(JSInterop.JSRuntime, loggerMock.Object);

        // Act
        await provider.UpdateStyleProperty("Element", "Property", "Value");

        // Assert
        JSInterop.VerifyInvoke("updateStyleProperty");
    }

    [Fact]
    public async Task UpdateStylePropertyBySelector_ShouldInvoke_UpdateStylePropertyBySelector_InModule()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<CommonJsFunctionsProvider>>();
        JSInterop.SetupModule("import", new string[] { "./_content/Craft.JsHelpers/commonMethods.js" });
        JSInterop.SetupVoid("updateStylePropertyBySelector", _ => true);
        JSInterop.Mode = JSRuntimeMode.Loose;

        var provider = new CommonJsFunctionsProvider(JSInterop.JSRuntime, loggerMock.Object);

        // Act
        await provider.UpdateStylePropertyBySelector("Selector", "Property", "Value");

        // Assert
        JSInterop.VerifyInvoke("updateStylePropertyBySelector");
    }
}
