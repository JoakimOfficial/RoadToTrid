using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Shared;

[XmlRoot(ElementName = "tris_file")]
public class TrisFile
{
    [XmlAttribute(AttributeName = "code")]
    public string Code { get; set; }

    [XmlText]
    public string Text { get; set; }
}
