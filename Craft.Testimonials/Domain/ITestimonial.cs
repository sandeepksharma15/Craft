using Craft.Domain.Contracts;

namespace Craft.Testimonials.Domain;

public interface ITestimonial : IEntity, IHasUser
{
    public string Name { get; set; }

    public string Feedback { get; set; }

    public int Rating { get; set; }

    public string SearchTags { get; set; }

    public bool IsApproved { get; set; }
}
