using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Shared;

[XmlRoot(ElementName = "term")]
public class Term
{
    [XmlAttribute(AttributeName = "type")]
    public string Type { get; set; }

    [XmlAttribute(AttributeName = "code")]
    public string Code { get; set; }

    [XmlText]
    public string Text { get; set; }
}
