using Craft.Core.Repositories;
using Craft.CustomerFeedback.Domain;
using Craft.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Craft.CustomerFeedback.Services;

public interface ICustFeedbackService : IRepository<CustFeedback>
{
}

public class CustFeedbackService : Repository<CustFeedback>, ICustFeedbackService
{
    public CustFeedbackService(DbContext appDbContext, ILogger
        <Repository<CustFeedback, KeyType>> logger) : base(appDbContext, logger)
    {
    }
}
