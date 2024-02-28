using System.Reflection;
using FluentAssertions;

namespace Craft.Utilities.Helpers;

public static class TestHelpers
{
    /// <summary>
    /// Helper Method to Compare If Two Objects Have The Same Set Of Data
    /// </summary>
    /// <typeparam name="T">The Base Type Of Two Objects</typeparam>
    /// <param name="one">One Object Derived From T</param>
    /// <param name="another">Another Object Derived From T</param>
    public static void AreTheySame<T>(T one, T another)
    {
        ArgumentNullException.ThrowIfNull(one, nameof(one));
        ArgumentNullException.ThrowIfNull(another, nameof(another));

        foreach (PropertyInfo field in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            object oneValue = field.GetValue(obj: one);
            object anotherValue = field.GetValue(obj: another);

            Type type = field.PropertyType;

            if ((type != typeof(string) && typeof(IEnumerable<string>).IsAssignableFrom(type)) || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)))
                oneValue.Should().BeEquivalentTo(anotherValue);
            else
                oneValue.Should().Be(anotherValue);
        }
    }

    /// <summary>
    /// Helper Method to Compare If Another Object Is Having Same Set Of Data As This One
    /// </summary>
    /// <typeparam name="T">The Base Type</typeparam>
    /// <param name="one">THIS Object Derived From T</param>
    /// <param name="another">Another Object Derived From T</param>
    public static void ShouldBeSameAs<T>(this T one, T another)
        => AreTheySame(one, another);
}
