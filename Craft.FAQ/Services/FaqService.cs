using Craft.Core.Repositories;
using Craft.Domain.Repositories;
using Craft.FAQ.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Craft.FAQ.Services;

public interface IFaqService : IRepository<FaqSection>
{
    Task<List<FaqSection>> GetSectionsAsync(CancellationToken cancellationToken = default);
}

public class FaqService : Repository<FaqSection>, IFaqService
{
    public FaqService(DbContext appDbContext, ILogger<Repository<FaqSection, KeyType>> logger)
        : base(appDbContext, logger) { }

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
}
