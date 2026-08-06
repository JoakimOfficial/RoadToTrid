using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.MarcXmlModels;

[XmlRoot(ElementName = "collection", Namespace = "http://www.loc.gov/MARC21/slim")]
public class MarcCollectionModel
{
    [XmlElement(ElementName = "record")]
    public List<MarcRecordModel> Records { get; set; }

    // // These attributes are rarely needed explicitly unless you handle them specifically in your code
    // [XmlAttribute(AttributeName = "xsi", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
    // public string Xsi { get; set; }

    // [XmlAttribute(AttributeName = "schemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
    // public string SchemaLocation { get; set; }

    // [XmlAttribute(AttributeName = "xmlns")]
    // public string Xmlns { get; set; }
}