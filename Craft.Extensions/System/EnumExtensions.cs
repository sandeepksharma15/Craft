using System.ComponentModel;
using System.Reflection;

namespace System;

public static class EnumExtensions
{
    public static bool Contains<T>(this string agent, T flags) where T : Enum
    {
        return EnumValues<T>.TryGetSingleName(flags, out var value) && value != null
            ? agent.Contains(value, StringComparison.InvariantCultureIgnoreCase)
            : flags
                .GetFlags()
                .Any(item => agent.Contains(item.ToStringInvariant(), StringComparison.InvariantCultureIgnoreCase));
    }

    public static string GetDescription<T>(this T someEnum)
    {
        if (someEnum is Enum)
        {
            MemberInfo[] memberInfo = someEnum.GetType().GetMember(someEnum.ToString()!);

            if (memberInfo?.Length > 0)
            {
                object[] attributess = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attributess?.Length > 0)
                    return ((DescriptionAttribute)attributess[0]).Description;
                else
                    return someEnum.ToString()!;
            }
        }

        return string.Empty;
    }

    public static IEnumerable<T> GetFlags<T>(this T value) where T : Enum
        => EnumValues<T>
            .GetValues()
            .Where(item => value.HasFlag(item));

    public static bool IsSet(this Enum input, Enum matchTo)
        => (Convert.ToUInt32(input) & Convert.ToUInt32(matchTo)) != 0;

    public static T ToEnum<T>(this int value) where T : struct
    {
        Type enumType = typeof(T);
        if (!enumType.IsEnum)
            throw new Exception("T must be an Enumeration type.");

        return (T)Enum.ToObject(enumType, value);
    }

    public static T ToEnum<T>(this string value, bool ignoreCase = true) where T : struct
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(nameof(value));

        Type enumType = typeof(T);
        if (!enumType.IsEnum)
            throw new Exception("T must be an Enumeration type.");

        return (T)Enum.Parse(typeof(T), value, ignoreCase);
    }

    public static string ToStringInvariant<T>(this T value) where T : Enum
        => EnumValues<T>.GetName(value);
}

/// <summary>
/// A utility class for working with enumeration types in C#. It provides methods to retrieve
/// names, descriptions, values, and other information associated with enum values.
/// </summary>
/// <typeparam name="T">The enum type.</typeparam>
public static class EnumValues<T> where T : Enum
{
    // Static dictionaries to store enum values, names, and descriptions.
    private static readonly Dictionary<T, string> Description = [];
    private static readonly Dictionary<T, string> Names = [];
    private static readonly T[] Values;

    /// <summary>
    /// Initializes static members of the EnumValues class.
    /// Retrieves enum values, names, and descriptions and populates the dictionaries.
    /// </summary>
    static EnumValues()
    {
        Type type = typeof(T);

        // Check if T is of type System.Enum
        if (type.BaseType != typeof(Enum))
            throw new ArgumentException("T must be of type System.Enum");

        // Get all enum values
        Values = (T[])Enum.GetValues(type);

        // Populate dictionaries
        Names = Values.ToDictionary(value => value, value => value.ToString());
        Description = Values.ToDictionary(value => value, value => value.GetDescription());
    }

    /// <summary>
    /// Gets the description of a specific enum value.
    /// If the description is not found, returns a comma-separated string of flag descriptions.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <returns>The description of the enum value.</returns>
    public static string GetDescription(T value)
        => Description.TryGetValue(value, out var result)
           ? result
           : string.Join(',', value.GetFlags().Select(x => Description[x]));

    /// <summary>
    /// Gets a dictionary of all enum values and their descriptions.
    /// </summary>
    /// <returns>A dictionary of enum values and descriptions.</returns>
    public static Dictionary<T, string> GetDescriptions()
        => Description;

    /// <summary>
    /// Gets the name of a specific enum value.
    /// If the name is not found, returns a comma-separated string of flag names.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <returns>The name of the enum value.</returns>
    public static string GetName(T value)
        => Names.TryGetValue(value, out var result)
               ? result
               : string.Join(',', value.GetFlags().Select(x => Names[x]));

    /// <summary>
    /// Gets a dictionary of all enum values and their names.
    /// </summary>
    /// <returns>A dictionary of enum values and names.</returns>
    public static Dictionary<T, string> GetNames()
        => Names;

    /// <summary>
    /// Gets an array of all enum values.
    /// </summary>
    /// <returns>An array of enum values.</returns>
    public static T[] GetValues()
        => Values;

    /// <summary>
    /// Tries to get the description of a specific enum value.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <param name="result">The description of the enum value if found.</param>
    /// <returns>True if the description is found; otherwise, false.</returns>
    public static bool TryGetSingleDescription(T value, out string result)
        => Description.TryGetValue(value, out result);

    /// <summary>
    /// Tries to get the name of a specific enum value.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <param name="result">The name of the enum value if found.</param>
    /// <returns>True if the name is found; otherwise, false.</returns>
    public static bool TryGetSingleName(T value, out string result)
            => Names.TryGetValue(value, out result);
}
