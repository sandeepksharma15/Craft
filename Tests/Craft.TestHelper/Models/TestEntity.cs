using Craft.Domain.Contracts;

namespace Craft.TestHelper.Models;

public class TestEntity : IEntity, IModel
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
