using System;
/// <summary> Extensions for various number types. </summary>
public static class NumberExtensions
{
    /// <summary>
    ///     Formats a large number to 5 digits (2 decimal places) with a suffix. (e.g. 123,456,789,123 (123 Billion) ->
    ///     123.45B)
    /// </summary>
    /// <param name="numberToFormat"> The number to format. </param>
    /// <param name="decimalPlaces"> The number of decimal places to include - <i> defaults to <c> 2 </c> </i> </param>
    /// <returns> A <see cref="string" />. </returns>
    public static string FormatNumber(this long numberToFormat, int decimalPlaces = 0)
    {
        // Get the default string representation of the numberToFormat.
        string numberString = numberToFormat.ToString();

        foreach (NumberSuffix suffix in Enum.GetValues(typeof(NumberSuffix)))
        {
            // Assign the amount of digits to base 10.
            double currentValue = 1 * Math.Pow(10, (int)suffix * 3);

            // Get the suffix value.
            string suffixValue = Enum.GetName(typeof(NumberSuffix), (int)suffix);

            // If the suffix is the placeholder, set it to an empty string.
            if ((int)suffix == 0) { suffixValue = string.Empty; }

            // Set the return value to a rounded value with the suffix.
            if (numberToFormat >= currentValue)
            {
                numberString = $"{Math.Round(numberToFormat / currentValue, decimalPlaces, MidpointRounding.ToEven)}{suffixValue}";
            }
        }

        return numberString;
    }

    /// <summary> Suffixes for numbers based on how many digits they have left of the decimal point. </summary>
    /// <remarks> The order of the suffixes matters! </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "UnusedMember.Local")]
    private enum NumberSuffix
    {
        /// <summary> A placeholder if the value is under 1 thousand </summary>
        P,

        /// <summary> Thousand </summary>
        K,

        /// <summary> Million </summary>
        M,

        /// <summary> Billion </summary>
        B,

        /// <summary> Trillion </summary>
        T,

        /// <summary> Quadrillion </summary>
        Q
    }
}