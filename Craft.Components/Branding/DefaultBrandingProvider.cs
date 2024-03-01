using Craft.Core.DependencyInjection;

namespace Craft.Components.Branding;

public class DefaultBrandingProvider : IBrandingProvider, ITransientDependency
{
    public virtual string AppName => "AppFlow";

    public virtual string LogoReverseUrl => null;

    public virtual string LogoUrl => null;
}
