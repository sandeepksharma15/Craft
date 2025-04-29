using Craft.Domain.Repositories;
using Craft.FAQ.Domain;

namespace Craft.FAQ.Services;

public interface IFaqService : IRepository<FaqSection>
{
    Task<List<FaqSection>> GetSectionsAsync(CancellationToken cancellationToken = default);
    List<FaqSection?> GetFilteredSections(List<FaqSection?> sections, string searchText = null);
    Task<List<FaqSection?>> GetFilteredSections(string searchText = null);
}
