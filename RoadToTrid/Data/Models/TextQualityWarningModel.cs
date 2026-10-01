using RoadToTrid.Data.Enums;

namespace RoadToTrid.Data.Models;

public class TextQualityWarningModel
{
    public string RecordNumber { get; set; } = string.Empty;
    public TextQualityWarningLevel Level { get; set; } = TextQualityWarningLevel.Warning;
    public string Message { get; set; } = string.Empty;
    public List<string> Issues { get; set; } = [];
    public string SuggestedText { get; set; } = string.Empty;
}
