using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Shared;

[XmlRoot(ElementName = "abstract")]
public class Abstract
{
    [XmlAttribute(AttributeName = "original")]
    public string Original { get; set; }

    [XmlText]
    public string Text { get; set; }
}
