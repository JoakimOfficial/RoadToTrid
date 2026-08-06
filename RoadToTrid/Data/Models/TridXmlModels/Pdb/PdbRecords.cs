using RoadToTrid.Data.Interfaces;
using RoadToTrid.Data.Models.TridXmlModels.Bibcat;
using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Pdb;

[XmlRoot(ElementName = "records")]
public class PdbRecords : IRecords
{
    [XmlElement(ElementName = "record")]
    public List<PdbRecord> Records { get; set; } = new List<PdbRecord>();

    [XmlAttribute(AttributeName = "noNamespaceSchemaLocation", Namespace = "http://www.w3.org/2001/XMLSchema-instance")]
    public string NoNamespaceSchemaLocation { get; set; } = "https://trisdataentry.trb.org/xml/xsd/TRISrecords_export.xsd http://www.transportportal.se/trb/Transguide_attributes.xsd";
}