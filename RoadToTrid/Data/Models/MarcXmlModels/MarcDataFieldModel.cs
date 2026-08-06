using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.MarcXmlModels;

[XmlRoot(ElementName = "datafield")]
public class MarcDataFieldModel
{
    [XmlElement(ElementName = "subfield")]
    public List<MarcSubFieldModel> Subfields { get; set; }

    [XmlAttribute(AttributeName = "tag")]
    public string Tag { get; set; }

    [XmlAttribute(AttributeName = "ind1")]
    public string Ind1 { get; set; }

    [XmlAttribute(AttributeName = "ind2")]
    public string Ind2 { get; set; }
}
