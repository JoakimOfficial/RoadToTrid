using RoadToTrid.Data.Models;

namespace RoadToTrid.ViewModels;

public class HomeUploadViewModel
{
    public IReadOnlyList<XmlFileProcessingModel> Files { get; init; } = [];
    public string? ErrorMessage { get; init; }
}
