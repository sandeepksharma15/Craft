namespace System;

public static class OtherExtensions
{
    /// <summary>
    /// Converts a decimal value to its percentage representation with two decimal places.
    /// </summary>
    /// <param name="value">The decimal value to be converted.</param>
    /// <returns>The percentage representation of the decimal value.</returns>
    public static string ToPercentage(this decimal value)
        => (value * 100).ToString("0.##") + "%";

    /// <summary>
    /// Converts a double value to its percentage representation with two decimal places.
    /// </summary>
    /// <param name="value">The double value to be converted.</param>
    /// <returns>The percentage representation of the double value.</returns>
    public static string ToPercentage(this double value)
        => (value * 100).ToString("0.##") + "%";

    /// <summary>
    /// Converts a float value to its percentage representation with two decimal places.
    /// </summary>
    /// <param name="value">The float value to be converted.</param>
    /// <returns>The percentage representation of the float value.</returns>
    public static string ToPercentage(this float value)
        => (value * 100).ToString("0.##") + "%";
}
