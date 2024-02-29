using Craft.Core.DependencyInjection;

namespace Craft.Infrastructure.Emailing;

public interface IEmailTemplateService : ITransientDependency
{
    string GenerateEmailTemplate<T>(string templateName, T mailTemplateModel);
}
