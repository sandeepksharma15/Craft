using Craft.FAQ.Domain;
using Craft.FAQ.Services;
using Craft.UiComponents.Components;
using Microsoft.AspNetCore.Components;

namespace Craft.FAQ.UI;

public partial class FaqList
{
    #region Paramaters & Injects

    [Inject] public required IFaqService FaqService { get; set; }

    [Parameter] public ShowProcessing? ShowProcessRef { get; set; } = null!;

    #endregion

    #region Member Fields

    private List<FaqSection?> _sections = [];
    private List<FaqSection?> _filteredSections = [];

    #endregion

    protected override async Task OnInitializedAsync()
    {
        // Start The Process
        ShowProcessRef?.StartProcessing();

        // Get The Faq Sections
        _sections = await FaqService.GetSectionsAsync();
        _filteredSections = FaqService.GetFilteredSections(_sections, null) ?? [];

        // Finish The Process
        ShowProcessRef?.StopProcessing();
    }

    private void OnSearchTextChange(string searchText)
    {
        // Start The Process
        ShowProcessRef?.StartProcessing();

        // Get The Faq Sections
        _filteredSections = FaqService.GetFilteredSections(_sections, searchText) ?? [];

        // Finish The Process
        ShowProcessRef?.StopProcessing();

        // Update the UI
        StateHasChanged();
    }
}
