using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Shared;

[XmlRoot(ElementName = "language_2")]
public class Language2
{
    [XmlAttribute(AttributeName = "id")]
    public string Id { get; set; }

    [XmlAttribute(AttributeName = "code")]
    public string Code { get; set; }

    [XmlText]
    public string Text { get; set; }
}
