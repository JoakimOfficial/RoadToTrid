namespace RoadToTrid.Helpers;

public static class DictionaryHelper
{
    public static string GetKey(Dictionary<string, string> dictionary, string value)
    {
        // Iterate through the dictionary to find the first key with the specified value
        foreach (KeyValuePair<string, string> keyValuePair in dictionary)
        {
            if (keyValuePair.Value == value)
            {
                return keyValuePair.Key;
            }
        }

        // Return null if no matching value is found
        return "";
    }

    // Generic method to get a value from any dictionary by key
    public static string GetValue(Dictionary<string, string> dictionary, string key)
    {
        // Attempts to retrieve the value; returns null if the key doesn't exist
        return dictionary.TryGetValue(key, out string value) ? value : "";
    }
}
