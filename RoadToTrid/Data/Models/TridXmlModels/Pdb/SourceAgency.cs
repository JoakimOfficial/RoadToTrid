using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Pdb;

[XmlRoot(ElementName = "source_agency")]
public class SourceAgency
{
    [XmlAttribute(AttributeName = "city")]
    public string City { get; set; } = "Linköping";

    [XmlAttribute(AttributeName = "country_name")]
    public string CountryName { get; set; } = "Sweden";

    [XmlAttribute(AttributeName = "postal_code")]
    public string PostalCode { get; set; } = "SE-581 95";

    [XmlAttribute(AttributeName = "site_url")]
    public string SiteUrl { get; set; } = "http://www.vti.se/en/";

    [XmlText]
    public string AgencyName { get; set; } = "Swedish National Road and Transport Research Institute(VTI)";
}
