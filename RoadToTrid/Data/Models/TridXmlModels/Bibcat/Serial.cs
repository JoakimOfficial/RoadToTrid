using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Bibcat;

[XmlRoot(ElementName = "serial")]
public class Serial
{
    [XmlAttribute(AttributeName = "publisher")]
    public string Publisher { get; set; } = "";

    [XmlAttribute(AttributeName = "editor")]
    public string Editor { get; set; } = "";

    [XmlAttribute(AttributeName = "issn")]
    public string Issn { get; set; } = "";

    [XmlAttribute(AttributeName = "eissn")]
    public string Eissn { get; set; } = "";

    [XmlAttribute(AttributeName = "oclc")]
    public string Oclc { get; set; } = "";

    [XmlAttribute(AttributeName = "serialurl")]
    public string SerialUrl { get; set; } = "";

    [XmlText]
    public string Text { get; set; } = "";
}