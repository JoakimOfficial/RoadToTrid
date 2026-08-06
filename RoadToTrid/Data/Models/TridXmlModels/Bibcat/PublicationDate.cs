using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Bibcat;

[XmlRoot(ElementName = "publication_date")]
public class PublicationDate
{
    [XmlAttribute(AttributeName = "year")]
    public string Year { get; set; }

    [XmlAttribute(AttributeName = "month")]
    public string Month { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "day")]
    public string Day { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "approximate")]
    public string Approximate { get; set; } = string.Empty;

    [XmlText]
    public string Text { get; set; }
}
