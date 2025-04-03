using Craft.QuerySpec.Core;
using Craft.Security.CurrentUserService;
using Craft.Testimonials.Domain;
using Craft.Testimonials.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace Craft.Testimonials.UI;

public partial class ListTestimonials
{
    #region Parameters & Injects

    [Inject] public required ILogger<ListTestimonials>? Logger { get; set; }

    [Inject] public required ICurrentUser CurrentUser { get; set; }
    [Inject] public required ITestimonialService TestimonialService { get; set; }

    [Parameter] public EventCallback<KeyType> OnTestimonialDelete { get; set; }
    [Parameter] public EventCallback<KeyType> OnTestimonialEdit { get; set; }

    #endregion

    #region Private Members

    private List<Testimonial> _testimonials = [];
    private bool? _filterApproved { get; set; } = null;
    private MudTable<Testimonial>? _table;
    private int _selectedRadio = 0;

    #endregion

    protected override async Task OnInitializedAsync()
    {
        _testimonials = await TestimonialService.GetAllAsync();
    }

    private async Task<TableData<Testimonial>> LoadServerData(TableState state, CancellationToken token)
    {
        var query = new Query<Testimonial>()
            .Where(x => _filterApproved == null || x.IsApproved == _filterApproved)
            .Skip(state.Page * state.PageSize)
            .Take(state.PageSize);  

        var items = await TestimonialService.GetAllAsync(query, token);

        var totalItems = await TestimonialService.GetCountAsync(query, token);

        return new TableData<Testimonial> { Items = items, TotalItems = (int)totalItems };
    }

    private async Task DeleteTestimonial(KeyType id)
    {
        if (OnTestimonialDelete.HasDelegate)
            await OnTestimonialDelete.InvokeAsync(id);

        await _table!.ReloadServerData();
    }

    private async Task EditTestimonial(KeyType id)
    {
        if (OnTestimonialEdit.HasDelegate)
            await OnTestimonialEdit.InvokeAsync(id);
    }

    private async Task  OnFilterChange(int newValue)
    {
        _selectedRadio = newValue;

        if (newValue == 0)
            _filterApproved = null;
        else if (newValue == 1)
            _filterApproved = true;
        else
            _filterApproved = false;

        await _table!.ReloadServerData();
    }
}
