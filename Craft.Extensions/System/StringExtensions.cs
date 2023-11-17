namespace System;

public static class StringExtensions
{
    /// <summary>
    /// Removes extra spaces between words and trims the input string.
    /// </summary>
    /// <param name="input">The input string to be processed.</param>
    /// <returns>A string with no more than a single space between words, trimmed.</returns>
    public static string RemoveExtraSpaces(this string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        // Split the string into words and remove empty entries
        var words = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        // Join the words with a single space
        return string.Join(" ", words);
    }

    /// <summary>
    /// Extension method for strings that extracts and returns the substring after the last occurrence
    /// of a specified delimiter. If the source string is null, the method returns null.
    /// </summary>
    /// <param name="source">The input string.</param>
    /// <param name="delimiter">The delimiter to identify the last occurrence (default is '.').</param>
    /// <returns>
    /// The substring after the last delimiter in the source string, or the entire string if the
    /// delimiter is not found. Returns null if the source string is null.
    /// </returns>
    public static string GetStringAfterLastDelimiter(this string source, char delimiter = '.')
    {
        if (source == null) return null;

        int lastDelimiterIndex = source.LastIndexOf(delimiter);

        return lastDelimiterIndex >= 0 ? source[(lastDelimiterIndex + 1)..] : source;
    }

    /// <summary>
    /// Checks whether the specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>
    ///   <c>true</c> if the string is null, empty, or consists only of white-space characters; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsEmpty(this string value) => string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Checks whether the specified string is non-empty, meaning it is not null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="value">The string to check.</param>
    /// <returns>
    ///   <c>true</c> if the string is non-empty; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNonEmpty(this string value) => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Checks whether the specified string is null or empty.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <returns>
    ///   <c>true</c> if the string is null or empty; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrEmpty(this string str) => string.IsNullOrEmpty(str);

    /// <summary>
    /// Indicates whether a specified string is null, empty, or consists only of white-space characters.
    /// </summary>
    /// <param name="str">The string to check.</param>
    /// <returns>
    ///   <c>true</c> if the string is null, empty, or consists only of white-space characters; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsNullOrWhiteSpace(this string str) => string.IsNullOrWhiteSpace(str);
}
