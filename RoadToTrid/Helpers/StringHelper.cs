using RoadToTrid.Data.Mappings;
using System.Text;
using System.Text.RegularExpressions;

namespace RoadToTrid.Helpers;

/// <summary>
/// Provides utility methods for common string manipulations, including character extraction, 
/// pagination conversion, encoding, XML character replacement, and whitespace normalization.
/// </summary>
/// <remarks>
/// The <see cref="StringHelper"/> class is designed to handle various string operations that are
/// frequently used in text processing and encoding tasks. Each method is static, allowing 
/// them to be accessed directly through the class without requiring an instance.
/// </remarks>
/// <example>
/// Example usage:
/// <code>
/// string firstChar = StringHelper.GetFirstCharacter("Hello"); // Output: "H"
/// string pagination = StringHelper.ConvertSwedishPaginationToEnglish("s. 45"); // Output: "p. 45"
/// byte[] encodedBytes = StringHelper.EncodeStringToBytes("Sample text");
/// string xmlSafeString = StringHelper.ReplaceSpecialCharsForXml("<tag>Value</tag>");
/// string normalizedWhitespace = StringHelper.ReplaceInvisibleCharacters("This text contains​invisible​characters.");
/// </code>
/// </example>
public static class StringHelper
{
    /// <summary>
    /// Returns a new string containing only the first character of the input string.
    /// If the input is null or empty, returns an empty string.
    /// </summary>
    /// <param name="input">The input string to process.</param>
    /// <returns>A string containing the first character or an empty string if input is null or empty.</returns>
    public static string GetFirstCharacter(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        return input[0].ToString();
    }

    /// <summary>
    /// Replaces all occurrences of " s." with "p" in the given input string,
    /// converting Swedish pagination notation to English.
    /// </summary>
    /// <param name="input">The input string to process.</param>
    /// <returns>A new string with all " s." replaced by "p", or an empty string if input is null.</returns>
    public static string ConvertSwedishPaginationToEnglish(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        if (input[0] == 's')
        {
            return input.Replace("s.", "pp");
        }

        return input.Replace(" s.", "p");
    }

    /// <summary>
    /// Encodes the specified string into a byte array using UTF-8 encoding.
    /// </summary>
    /// <param name="input">The string to encode into bytes.</param>
    /// <returns>A byte array representing the UTF-8 encoded string.</returns>
    public static byte[] EncodeStringToBytes(string input)
    {
        return Encoding.UTF8.GetBytes(input);
    }

    /// <summary>
    /// Encodes special characters in the input string with their XML-safe representations
    /// based on predefined character mappings.
    /// </summary>
    /// <param name="input">The string to process and replace special characters in.</param>
    /// <returns>A new string with special characters encoded for XML compatibility.</returns>
    /// <remarks>
    /// This method iterates through the <see cref="CharacterMappings.Characters"/> dictionary 
    /// and replaces each key found in the input string with its corresponding value.
    /// </remarks>
    public static string EncodeSpecialCharsForXml(string input)
    {
        foreach (KeyValuePair<string, string> character in CharacterMappings.Characters)
        {
            input = input.Replace(character.Key, character.Value);
        }

        return input;
    }

    /// <summary>
    /// Cleans a string by replacing specific characters with their ASCII equivalents
    /// and removing invisible characters for XML compatibility.
    /// </summary>
    /// <param name="input">The string to sanitize.</param>
    /// <returns>A cleaned string safe for XML processing.</returns>
    public static string CleanStringForXml(string input)
    {
        if (string.IsNullOrEmpty(input)) { 
            return input;
        }

        // Replace specific characters with ASCII equivalents
        input = input.Replace("’", "'");
        //.Replace("\u2011", "-")
        //.Replace("\u2014", "-")
        //.Replace("\u2013", "-");

        //input = input.Replace(" 1)", "1.").Replace(" 2)", "2.").Replace(" 3)", "3.")

        input = Regex.Replace(input, @"\b(\d+)\)", "$1.");

        input = Regex.Replace(input, @"[\u2011\u2013\u2014]", "-");

        // Remove invisible Unicode characters
        input = Regex.Replace(input, @"[\u00A0\u2001\u200B\u200C]", " ");

        return input;
    }


    public static string GetDatePart(string dateString, string type)
    {
        string[] dateArray = dateString.Split("--");

        return type == "startDate" ? dateArray[0].Trim() : dateArray[1].Trim();
    }

    public static string RemoveBracketsFromYear(string year)
    {
        return year.Replace("[", "").Replace("]", "");
    }

    public static bool IsLikelySwedish(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        // Check for Swedish-specific characters
        string swedishChars = "åäöÅÄÖ";
        return text.Any(c => swedishChars.Contains(c));
    }

    /// <summary>
    /// Removes the surrounding parentheses from the input string if present.
    /// </summary>
    /// <param name="input">The string to process.</param>
    /// <returns>
    /// A new string without the first and last characters if they are '(' and ')', 
    /// otherwise the original string.
    /// </returns>
    public static string RemoveSurroundingParentheses(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        if (input.Length >= 2 && input[0] == '(' && input[^1] == ')')
        {
            return input[1..^1];
        }

        return input;
    }
}
