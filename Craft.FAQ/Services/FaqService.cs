using Craft.Core.Repositories;
using Craft.FAQ.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Craft.FAQ.Services;

public class FaqService : Repository<FaqSection>, IFaqService
{
    public FaqService(DbContext appDbContext, ILogger<Repository<FaqSection, KeyType>> logger)
        : base(appDbContext, logger) { }

    /// <summary>
    /// Asynchronously retrieves a list of FAQ sections, including their associated queries and subsections.
    /// </summary>
    /// <remarks>The returned list is ordered by the section's ID and is retrieved without tracking changes in
    /// the database context.</remarks>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the operation. The default value is <see
    /// langword="default"/>.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="FaqSection"/>
    /// objects, each including their related queries and subsections.</returns>
    public Task<List<FaqSection>> GetSectionsAsync(CancellationToken cancellationToken = default)
    {
        return _appDbContext.Set<FaqSection>()
            .Include(x => x.Queries)
            .Include(x => x.SubSections)
                .ThenInclude(x => x.Queries)
            .OrderBy(x => x.Id)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Filters a list of FAQ sections based on the specified search text. It uses provided List of FAQ sections avoiding DB Trips.
    /// </summary>
    /// <remarks>A section is included in the result if it contains at least one query or subsection that
    /// matches the search text. Matching is performed on the question and answer text of queries, and the search is
    /// case-insensitive.</remarks>
    /// <param name="sections">The list of <see cref="FaqSection"/> objects to filter. Each section may contain queries and subsections.</param>
    /// <param name="searchText">The text to search for within the FAQ sections. If <see langword="null"/> or whitespace, the method returns the
    /// original list of sections.</param>
    /// <returns>A filtered list of <see cref="FaqSection"/> objects containing only the sections, queries, and subsections that
    /// match the specified search text. If no matches are found, an empty list is returned.</returns>
    public List<FaqSection> GetFilteredSections(List<FaqSection> sections, string searchText = null)
    {
        if (string.IsNullOrWhiteSpace(searchText))
            return sections;

        string lower = searchText.ToLower();

        return [.. sections
            .Where(section => section != null)
            .Select(section =>
            {
                // Filter top-level FAQs
                var matchingFaqs = section!.Queries?
                    .Where(q => IsMatch(q.Question, q.Answer, searchText))
                    .ToList();

                // Filter subsections with matching FAQs
                var matchingSubSections = section.SubSections?
                    .Select(sub => new FaqSubSection
                    {
                        Id = sub.Id,
                        Title = sub.Title,
                        Queries = [.. sub.Queries
                            .Where(q => IsMatch(q.Question, q.Answer, searchText))]
                    })
                    .Where(sub => sub.Queries.Count > 0)
                    .ToList();

                // Return section only if it has matches
                if ((matchingFaqs?.Count > 0) || (matchingSubSections?.Count > 0))
                {
                    return new FaqSection
                    {
                        Id = section.Id,
                        Title = section.Title,
                        Queries = matchingFaqs,
                        SubSections = matchingSubSections
                    };
                }

                return null;
            })
            .Where(filteredSection => filteredSection != null)];
    }

    /// <summary>
    /// Retrieves a filtered list of FAQ sections based on the specified search text. This makesa a DB trip to get the sections.
    /// </summary>
    /// <param name="searchText">The text to filter the FAQ sections by. If <see langword="null"/> or empty, all sections are returned.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a list of <see cref="FaqSection"/>
    /// objects that match the specified search text.</returns>
    public async Task<List<FaqSection>> GetFilteredSections(string searchText = null)
    {
        var sections = await GetSectionsAsync();

        return GetFilteredSections(sections, searchText);
    }

    private static bool IsMatch(string question, string answer, string searchText)
    {
        if (searchText.IsNullOrWhiteSpace())
            return true;

        var lower = searchText.ToLower();

        return question.Contains(lower, StringComparison.CurrentCultureIgnoreCase)
            || answer.Contains(lower, StringComparison.CurrentCultureIgnoreCase);
    }
}
