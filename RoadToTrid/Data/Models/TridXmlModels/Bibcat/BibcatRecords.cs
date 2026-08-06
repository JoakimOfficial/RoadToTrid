using RoadToTrid.Data.Interfaces;
using System.Xml;
using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Bibcat;

[XmlRoot(ElementName = "records")]
public class BibcatRecords : IRecords
{
    [XmlElement(ElementName = "record")]
    public List<BibcatRecord> Records { get; set; } = new List<BibcatRecord>();

    [XmlAttribute(AttributeName = "noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
    public string NoNamespaceSchemaLocation { get; set; } = "https://trisdataentry.trb.org/xml/xsd/TRISrecords_export.xsd http://www.transportportal.se/trb/Transguide_attributes.xsd";
}

