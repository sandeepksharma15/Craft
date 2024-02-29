using Craft.Core.DependencyInjection;

namespace Craft.Infrastructure.Emailing;

public interface IMailService : ITransientDependency
{
    Task SendAsync(MailRequest request, CancellationToken cancellationToken = default);
}
