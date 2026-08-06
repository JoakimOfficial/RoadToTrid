using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Bibcat;

[XmlRoot(ElementName = "availability_agency")]
public class AvailabilityAgency
{
    [XmlAttribute(AttributeName = "street_1")]
    public string Street1 { get; set; } = String.Empty;

    [XmlAttribute(AttributeName = "street_2")]
    public string Street2 { get; set; } = String.Empty;

    [XmlAttribute(AttributeName = "city")]
    public string City { get; set; } = "Linköping";

    [XmlAttribute(AttributeName = "region")]
    public string Region { get; set; } = String.Empty;

    [XmlAttribute(AttributeName = "country_name")]
    public string CountryName { get; set; } = String.Empty;

    [XmlAttribute(AttributeName = "postal_code")]
    public string PostalCode { get; set; } = String.Empty;

    [XmlAttribute(AttributeName = "site_url")]
    public string SiteUrl { get; set; } = "https://bibliotek.vti.se/";

    [XmlAttribute(AttributeName = "part_number")]
    public string PartNumber { get; set; } = String.Empty;

    [XmlAttribute(AttributeName = "order_url")]
    public string OrderUrl { get; set; } = "http://www.vti.se/en/library";

    [XmlAttribute(AttributeName = "position")]
    public string Position { get; set; } = String.Empty;

    [XmlText]
    public string Text { get; set; } = "Swedish National Road and Transport Research Institute (VTI)";
}
