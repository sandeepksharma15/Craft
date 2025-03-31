using Craft.QuerySpec.Core;
using Craft.Testimonials.Domain;
using Craft.Testimonials.Services;
using Craft.UiComponents.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Craft.Testimonials.UI;

public partial class ShowTestimonials
{
    [Inject] public required ITestimonialService TestimonialService { get; set; }
    [Inject] private ILogger<ShowTestimonials>? _logger { get; set; }

    [Parameter] public int Count { get; set; } = 3;
    [Parameter] public string SearchText { get; set; } = string.Empty;
    [Parameter] public string CssClass { get; set; } = string.Empty;

    private List<Testimonial> _testimonials = [];
    private readonly string _iconHeartFilled = IconContent.HeartFilled;
    private readonly string _iconHeartBorder = IconContent.HeartBorder;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var query = new Query<Testimonial>()
                .Where(x => x.IsApproved)
                .OrderBy(x => Guid.NewGuid())
                .Take(Count);

            if (SearchText.IsNullOrEmpty() == false)
                query = query.Where(x => x.Feedback.Contains(SearchText));

            _testimonials = await TestimonialService.GetAllAsync(query);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
