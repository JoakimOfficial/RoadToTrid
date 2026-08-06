using RoadToTrid.Data.Interfaces;
using RoadToTrid.Data.Models.TridXmlModels.Shared;
using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Bibcat;

[XmlRoot(ElementName = "record")]
public class BibcatRecord : IRecord
{
    [XmlAttribute(AttributeName = "type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "timestamp")]
    public string Timestamp { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "database")]
    public string Database { get; set; } = "VTI Library catalogue";

    [XmlAttribute(AttributeName = "record_no")]
    public string RecordNo { get; set; }

    [XmlElement(ElementName = "accession_number")]
    public string AccessionNumber { get; set; } = "0";

    [XmlElement(ElementName = "record_type")]
    public string RecordType { get; set; }

    [XmlElement(ElementName = "record_status")]
    public string RecordStatus { get; set; } = "N";

    [XmlElement(ElementName = "title")]
    public string Title { get; set; }

    [XmlElement(ElementName = "foreign_title")]
    public string ForeignTitle { get; set; }

    [XmlElement(ElementName = "language_1")]
    public Language1 Language1 { get; set; } = new();

    [XmlElement(ElementName = "language_2")]
    public Language2? Language2 { get; set; }

    [XmlElement(ElementName = "language_3")]
    public Language3? Language3 { get; set; }

    [XmlArray("document_urls")]
    [XmlArrayItem("url")]
    public List<Url> DocumentUrls { get; set; } = [];

    [XmlArray("tris_files")]
    [XmlArrayItem("tris_file")]
    public List<TrisFile> TrisFiles { get; set; } = [];

    [XmlElement(ElementName = "abstract")]
    public Abstract Abstract { get; set; }

    [XmlArray("index_terms")]
    [XmlArrayItem("term")]
    public List<Term> IndexTerms { get; set; } = [];

    [XmlArray("subject_areas")]
    [XmlArrayItem("subject_area")]
    public List<SubjectArea> SubjectAreas { get; set; } = [];

    [XmlElement(ElementName = "document")]
    public Document Document { get; set; }

    // Automatically called by XmlSerializer
    public bool ShouldSerializeLanguage2()
    {
        // Only serialize if Language2 is not null
        return Language2 != null;
    }

    // Automatically called by XmlSerializer
    public bool ShouldSerializeLanguage3()
    {
        // Only serialize if Language3 is not null
        return Language3 != null;
    }
}
