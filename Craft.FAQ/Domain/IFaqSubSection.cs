using Craft.Domain.Contracts;

namespace Craft.FAQ.Domain;

public interface IFaqSubSection : IEntity
{
    string Title { get; set; }

    public KeyType FaqSectionId { get; set; }
}
