using RoadToTrid.Data.Enums;

namespace RoadToTrid.Data.Models;

public class InvalidAbstractModel
{
    public List<int> Indexes { get; set; } = [];
    public string AbstractText { get; set; } = string.Empty;
    public string RecordNumber { get; set; } = string.Empty;
    public AbstractWarningType WarningType { get; set; }
    public string? ExpectedLanguageCode { get; set; }
    public string? DetectedLanguageCode { get; set; }
    public int? ExpectedAbstractNumber { get; set; }
    public int? DetectedAbstractNumber { get; set; }
}
