using Craft.FAQ.Domain;
using Craft.FAQ.Services;
using Craft.UiComponents.Components;
using Microsoft.AspNetCore.Components;

namespace Craft.FAQ.UI;

public partial class FaqList
{
    #region Paramaters & Injects

    [Inject] public required IFaqService FaqRepository { get; set; }

    [Parameter] public ShowProcessing? ShowProcessRef { get; set; } = null!;

    #endregion

    #region Member Fields

    private List<FaqSection?> _sections = [];
    private List<FaqSection?> _filteredSections = [];
    private string _searchText = string.Empty;

    #endregion

    protected override async Task OnInitializedAsync()
    {
        // Start The Process
        ShowProcessRef?.StartProcessing();

        // Get The Faq Sections
        _sections = await FaqRepository.GetSectionsAsync();
        _filteredSections = GetFilteredSections() ?? [];

        // Finish The Process
        ShowProcessRef?.StopProcessing();
    }

    private List<FaqSection?> GetFilteredSections()
    {
        if (string.IsNullOrWhiteSpace(_searchText))
            return _sections;

        string lower = _searchText.ToLower();

        return [.. _sections
            .Where(section => section != null)
            .Select(section =>
            {
                // Filter top-level FAQs
                var matchingFaqs = section!.Queries?
                    .Where(q => IsMatch(q.Question, q.Answer))
                    .ToList();

                // Filter subsections with matching FAQs
                var matchingSubSections = section.SubSections?
                    .Select(sub => new FaqSubSection
                    {
                        Id = sub.Id,
                        Title = sub.Title,
                        Queries = [.. sub.Queries.Where(q => IsMatch(q.Question, q.Answer))]
                    })
                    .Where(sub => sub.Queries.Count > 0)
                    .ToList();

                // Return section only if it has matches
                if ((matchingFaqs?.Count > 0) || (matchingSubSections?.Count > 0))
                {
                    return new FaqSection
                    {
                        Id = section.Id,
                        Title = section.Title,
                        Queries = matchingFaqs,
                        SubSections = matchingSubSections
                    };
                }

                return null;
            })
            .Where(filteredSection => filteredSection != null)];
    }

    private bool IsMatch(string question, string answer)
    {
        if (string.IsNullOrWhiteSpace(_searchText))
            return true;

        var lower = _searchText.ToLower();

        return question.Contains(lower, StringComparison.CurrentCultureIgnoreCase) 
            || answer.Contains(lower, StringComparison.CurrentCultureIgnoreCase);
    }

    private void OnSearchTextChange(string searchText)
    {
        _searchText = searchText;

        // Start The Process
        ShowProcessRef?.StartProcessing();

        // Get The Faq Sections
        _filteredSections = GetFilteredSections() ?? [];

        // Finish The Process
        ShowProcessRef?.StopProcessing();

        // Update the UI
        StateHasChanged();
    }
}
