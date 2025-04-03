using Craft.Security.CurrentUserService;
using Craft.Testimonials.Domain;
using Craft.Testimonials.Services;
using Mapster;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace Craft.Testimonials.UI;

public partial class AddTestimonial
{
    #region Parameters & Injects

    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] private ILogger<AddTestimonial>? _logger { get; set; }
    [Inject] public required ICurrentUser CurrentUser { get; set; }
    [Inject] public required ITestimonialService TestimonialService { get; set; }

    [Parameter] public EventCallback OnTestimonialCancel { get; set; }
    [Parameter] public EventCallback OnTestimonialSave { get; set; }

    #endregion

    #region Private Members

    private readonly TestimonialVM _testimonialVM = new();
    private int? _currentVal;

    #endregion

    private async Task SaveTestimonial()
    {
        try
        {
            // Map The View Model To The Domain Model
            var testimonial = _testimonialVM.Adapt<Testimonial>();

            // Set The User Id
            testimonial.UserId = CurrentUser.Id;

            // Save The Testimonial
            await TestimonialService.AddAsync(testimonial);

            // Show A Success Message
            Snackbar.Add("Thanks for your valuable feedback!! Saved Successfully", Severity.Success);
        }
        catch (Exception)
        {
            _logger?.LogError("[AddTestimonial] Error Saving Testimonial");
            Snackbar.Add("Sorry! Encountered an error while saving your feedback", Severity.Error);
        }
        finally
        {
            // Let The Caller Know We Are Done
            if (OnTestimonialSave.HasDelegate)
                await OnTestimonialSave.InvokeAsync();
        }
    }

    private async Task CancelTestimonial()
    {
        // Let The Caller Know We Are Done
        if (OnTestimonialSave.HasDelegate)
            await OnTestimonialSave.InvokeAsync();
    }

    private void HandleHoveredValueChanged(int? val) => _currentVal = val;

    private string LabelText => (_currentVal ?? _testimonialVM!.Rating) switch
    {
        1 => "Bad",
        2 => "Needs Improvement(s)",
        3 => "Sufficient",
        4 => "Good",
        5 => "Awesome!",
        _ => "Rate Your Experience"
    };
}
