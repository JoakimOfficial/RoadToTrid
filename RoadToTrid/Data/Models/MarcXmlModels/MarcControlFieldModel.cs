using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.MarcXmlModels;

[XmlRoot(ElementName = "controlfield")]
public class MarcControlFieldModel
{
    [XmlAttribute(AttributeName = "tag")]
    public string Tag { get; set; }

    [XmlText]
    public string Text { get; set; }
}
