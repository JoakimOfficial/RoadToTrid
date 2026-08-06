using System.Reflection.Metadata;
using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Bibcat;

[XmlRoot(ElementName = "document")]
public class Document
{
    [XmlElement(ElementName = "media_type")]
    public string MediaType { get; set; }

    [XmlElement(ElementName = "pagination")]
    public string Pagination { get; set; }

    [XmlArray("authors")]
    [XmlArrayItem("author")]
    public List<Author> Authors { get; set; } = [];

    [XmlElement(ElementName = "monograph")]
    public Monograph Monograph { get; set; }
}
