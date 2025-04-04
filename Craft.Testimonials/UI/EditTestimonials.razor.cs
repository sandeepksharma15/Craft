using Craft.Security.CurrentUserService;
using Craft.Testimonials.Domain;
using Craft.Testimonials.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;
using Mapster;
using Microsoft.AspNetCore.Components.Web;

namespace Craft.Testimonials.UI;

public partial class EditTestimonials
{
    #region Parameters & Injects

    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ILogger<EditTestimonials>? Logger { get; set; }

    [Inject] public required ICurrentUser CurrentUser { get; set; }
    [Inject] public required ITestimonialService TestimonialService { get; set; }

    [Parameter] public required KeyType Id { get; set; }
    [Parameter] public required List<string> SearchTags { get; set; } = [];
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback<bool> OnSave { get; set; }

    #endregion

    #region Private Members

    private TestimonialVM _testimonialVM = new();
    private Testimonial? _testimonial = new();
    private IReadOnlyCollection<string> _selected = [];

    #endregion

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _testimonial = await TestimonialService.GetAsync(Id);
            _testimonialVM = _testimonial.Adapt<TestimonialVM>();

            // Preload _selected with values from _testimonialVM.SearchTags
            if (!string.IsNullOrEmpty(_testimonialVM.SearchTags))
            {
                _selected = [.. _testimonialVM.SearchTags.Split(',')];
            }
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "[EditTestimonial] Faced an error while retrieving Key: [{id}] Testimonial", Id);

            // Inform the Caller that the operation failed
            if (OnSave.HasDelegate)
                await OnSave.InvokeAsync(false);
        }
    }

    private async Task SaveTestimonial(MouseEventArgs args)
    {
        try
        {
            _testimonialVM.SearchTags = string.Join(",", _selected);

            _testimonial = _testimonialVM.Adapt<Testimonial>();
            await TestimonialService.UpdateAsync(_testimonial);

            // Inform the Caller that the operation was successful
            if (OnSave.HasDelegate)
                await OnSave.InvokeAsync(true);
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "[EditTestimonials] Error saving testimonial");

            // Inform the Caller that the operation failed
            if (OnSave.HasDelegate)
                await OnSave.InvokeAsync(false);
        }
    }

    private void CancelTestimonial(MouseEventArgs args)
    {
        if (OnCancel.HasDelegate)
            OnCancel.InvokeAsync();
    }
}
