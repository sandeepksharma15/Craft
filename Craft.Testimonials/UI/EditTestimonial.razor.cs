using Craft.Security.CurrentUserService;
using Craft.Testimonials.Domain;
using Craft.Testimonials.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;
using Mapster;

namespace Craft.Testimonials.UI;

public partial class EditTestimonial
{
    #region Parameters & Injects

    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ILogger<AddTestimonial>? Logger { get; set; }

    [Inject] public required ICurrentUser CurrentUser { get; set; }
    [Inject] public required ITestimonialService TestimonialService { get; set; }

    [Parameter] public required KeyType Id { get; set; }
    [Parameter] public EventCallback OnCancel { get; set; }
    [Parameter] public EventCallback OnSave { get; set; }

    #endregion

    #region Private Members

    private TestimonialVM _testimonialVM = new();
    private int? _currentVal;
    private Testimonial? _testimonial = new();

    #endregion

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _testimonial = await TestimonialService.GetAsync(Id);
            _testimonialVM = _testimonial.Adapt<TestimonialVM>();
        }
        catch (Exception ex)
        {
            Logger?.LogError(ex, "[EditTestimonial] Faced an error while retrieving Key: [{id}] Testimonial", Id);
            Snackbar.Add(message: "Testimonial couldn't be retrieved! Try later!!", severity: Severity.Error);
        }
    }
    private void CancelTestimonial(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        throw new NotImplementedException();
    }
    private Task SaveTestimonial(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        throw new NotImplementedException();
    }
}
