namespace RoadToTrid.Data.Mappings;

public static class CharacterMappings
{
    private static readonly Dictionary<string, string> characters = new(){
        // XML Predefined Entities
        { "<", "&lt;" },
        { ">", "&gt;" },

        // Random characters that need to be replaced
        { "×", "&#215;" },
        { "´", "'" },
        { "′", "&#8242;" }, // Prime

        // Ligatures
        { "ﬀ", "ff" },
        { "ﬃ", "ffi" },
        { "ﬁ", "fi" },
        { "ﬄ", "ffl" },
        { "ﬂ", "fl" },
    };

    public static Dictionary<string, string> Characters => characters;
}
