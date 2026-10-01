using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Bibcat;

[XmlRoot(ElementName = "availability_agency")]
public class AvailabilityAgency
{
    [XmlAttribute(AttributeName = "street_1")]
    public string? Street1 { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "street_2")]
    public string? Street2 { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "city")]
    public string? City { get; set; } = "Linköping";

    [XmlAttribute(AttributeName = "region")]
    public string? Region { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "country_name")]
    public string? CountryName { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "postal_code")]
    public string? PostalCode { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "site_url")]
    public string? SiteUrl { get; set; } = "https://bibliotek.vti.se/";

    [XmlAttribute(AttributeName = "part_number")]
    public string? PartNumber { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "order_url")]
    public string? OrderUrl { get; set; } = "http://www.vti.se/en/library";

    [XmlAttribute(AttributeName = "position")]
    public string? Position { get; set; } = string.Empty;

    [XmlText]
    public string? Text { get; set; } = "Swedish National Road and Transport Research Institute (VTI)";
}
