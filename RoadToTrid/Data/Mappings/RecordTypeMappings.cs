namespace RoadToTrid.Data.Mappings;

public static class RecordTypeMappings
{
    private static readonly Dictionary<string, string> recordTypes = new()
        {
            { "AVHANDLING", "Publication" },
            { "LIC.", "Publication" },
            { "Project", "Research project" },
            { "RAPPORT MM", "Publication" },
        };

    public static Dictionary<string, string> RecordTypes => recordTypes;
}
