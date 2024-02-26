using Craft.Auditing.Extensions;
using Craft.Domain.Base;

namespace Craft.Auditing.Helpers;

public static class AuditingHelpers
{
    public static IList<string> GetNonAuditableModels()
    {
        // Get the list of all the types inherited from class EntityBase
        var modelNames = AppDomain.CurrentDomain.GetAssemblies()
               .SelectMany(s => s.GetTypes())
               .Where(t => typeof(EntityBase).IsAssignableFrom(t)
                           && t.IsClass && !t.IsAbstract && t.HasAuditAttributeDisabled())
               .Select(t => t.Name)
               .ToList();

        return modelNames;
    }
}
