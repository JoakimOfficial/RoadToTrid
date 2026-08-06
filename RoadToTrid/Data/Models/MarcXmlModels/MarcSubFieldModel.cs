using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.MarcXmlModels;

[XmlRoot(ElementName = "subfield")]
public class MarcSubFieldModel
{
    [XmlAttribute(AttributeName = "code")]
    public string Code { get; set; }

    [XmlText]
    public string Text { get; set; }
}