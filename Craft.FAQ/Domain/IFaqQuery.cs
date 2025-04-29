using Craft.Domain.Contracts;

namespace Craft.FAQ.Domain;

public interface IFaqQuery : IEntity
{
    string Question { get; set; }
    string Answer { get; set; }

    public KeyType? FaqSectionId { get; set; }
    public KeyType? FaqSubSectionId { get; set; }
}
