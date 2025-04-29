using Craft.Domain.Contracts;

namespace Craft.FAQ.Domain;

public interface IFaqSection: IEntity
{
    string Title { get; set; }
}
