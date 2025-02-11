using Craft.Core.Repositories;
using Craft.Domain.Repositories;
using Craft.Testimonials.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Craft.Testimonials.Services;

public class TestimonialService : Repository<Testimonial>, ITestimonialService
{
    public TestimonialService(DbContext appDbContext, ILogger<Repository<Testimonial, KeyType>> logger) 
        : base(appDbContext, logger)
    {
    }
}

public interface ITestimonialService : IRepository<Testimonial>
{

}
