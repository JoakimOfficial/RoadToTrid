using RoadToTrid.Data.Models.TridXmlModels.Bibcat;

namespace RoadToTrid.Data.Models;

public class ApplicationSettings
{
    public AvailabilityAgency AvailabilityAgency { get; set; } = new();
    public TextQualityReviewSettings TextQualityReview { get; set; } = new();
}
