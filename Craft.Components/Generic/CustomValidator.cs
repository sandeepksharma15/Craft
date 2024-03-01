using System.Net.Http.Json;
using Craft.Domain.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Newtonsoft.Json.Linq;

namespace Craft.Components.Generic;

public class CustomValidator : ComponentBase
{
    private ValidationMessageStore _messageStore;

    [CascadingParameter] private EditContext CurrentEditContext { get; set; }
    [Inject] private ISnackbar Snackbar { get; set; }

    protected override void OnInitialized()
    {
        if (CurrentEditContext == null)
            throw new InvalidOperationException(
                    $"{nameof(CustomValidator)} requires a cascading " +
                    $"parameter of type {nameof(EditContext)}. " +
                    $"For example, you can use {nameof(CustomValidator)} " +
                    $"inside an {nameof(EditForm)}.");

        _messageStore = new ValidationMessageStore(CurrentEditContext);

        CurrentEditContext.OnValidationRequested += (s, e) =>
            _messageStore.Clear();
        CurrentEditContext.OnFieldChanged += (s, e) =>
            _messageStore.Clear(e.FieldIdentifier);
    }

    public void AddError(string key, string value)
    {
        _messageStore.Add(CurrentEditContext.Field(key), value);
    }

    public void ClearErrors()
    {
        _messageStore.Clear();
        CurrentEditContext.NotifyValidationStateChanged();
    }

    public async Task DisplayErrors(HttpResponseMessage responseMessage)
    {
        JObject errMsg = JObject.Parse(await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false));

        if (errMsg.SelectToken("detail") != null)
        {
            string status = errMsg!.SelectToken("status")?.Value<string>();
            string detail = errMsg!.SelectToken("detail")?.Value<string>();

            Snackbar.Add($"{status}: {detail}", Severity.Error);
        }
        else
            DisplayErrors(await responseMessage.Content.ReadFromJsonAsync<Dictionary<string, List<string>>>().ConfigureAwait(false));
    }

    public void DisplayErrors(ServerResponse responseMessage)
    {
        _messageStore.Add(CurrentEditContext.Field(string.Empty), responseMessage.Message);

        responseMessage.Errors?.ForEach(err => _messageStore.Add(CurrentEditContext.Field(string.Empty), err));

        CurrentEditContext.NotifyValidationStateChanged();
    }

    public void DisplayErrors(Dictionary<string, List<string>> errors)
    {
        foreach (KeyValuePair<string, List<string>> err in errors)
            _messageStore.Add(CurrentEditContext.Field(err.Key), err.Value);

        CurrentEditContext.NotifyValidationStateChanged();
    }

    public void DisplayErrors()
    {
        CurrentEditContext.NotifyValidationStateChanged();
    }
}
