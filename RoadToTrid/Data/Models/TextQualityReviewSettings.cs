using RoadToTrid.Data.Enums;

namespace RoadToTrid.Data.Models;

public class TextQualityReviewSettings
{
    public bool IsEnabled { get; set; }
    public string? OllamaUrl { get; set; } = "http://localhost:11434";
    public string? Model { get; set; } = "llama3.1";
    public TextQualityWarningLevel WarningLevel { get; set; } = TextQualityWarningLevel.Warning;
    public int MaxRecordsPerFile { get; set; } = 20;
    public int TimeoutSeconds { get; set; } = 15;
    public int MaxSecondsPerFile { get; set; } = 60;
}
