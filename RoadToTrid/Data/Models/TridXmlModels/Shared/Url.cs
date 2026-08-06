using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Shared;

[XmlRoot(ElementName = "url")]
public class Url
{
    [XmlAttribute(AttributeName = "type")]
    public string Type { get; set; }

    [XmlText]
    public string Text { get; set; }
}