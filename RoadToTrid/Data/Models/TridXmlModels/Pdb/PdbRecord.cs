using RoadToTrid.Data.Interfaces;
using RoadToTrid.Data.Models.TridXmlModels.Shared;
using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Pdb;

// This is the new class for Record when the source is from "Projektdatabasen".
public class PdbRecord : IRecord
{
    [XmlAttribute(AttributeName = "type")]
    public string Type { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "timestamp")]
    public string Timestamp { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "database")]
    public string Database { get; set; } = "VTI research database";

    [XmlAttribute(AttributeName = "record_no")]
    public string RecordNo { get; set; }

    [XmlElement(ElementName = "accession_number")]
    public string AccessionNumber { get; set; } = "0";

    [XmlElement(ElementName = "record_type")]
    public string RecordType { get; set; }

    [XmlElement(ElementName = "record_status")]
    public string RecordStatus { get; set; } = "N";

    [XmlArray("attributes")]
    [XmlArrayItem("attribute")]
    public List<Attribute> Attributes { get; set; } = [];

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

    [XmlElement(ElementName = "source_agency")]
    public SourceAgency SourceAgency { get; set; } = new();

    [XmlArray("source_data")]
    public List<string> SourceData { get; set; } = [];

    [XmlArray("tris_files")]
    [XmlArrayItem("tris_file")]
    public List<TrisFile> TrisFiles { get; set; } = [];

    [XmlArray("report_numbers")]
    [XmlArrayItem("report_number")]
    public List<string> ReportNumbers { get; set; } = [];

    [XmlArray("contract_numbers")]
    [XmlArrayItem("contract_number")]
    public List<string> ContractNumbers { get; set; } = [];

    [XmlElement(ElementName = "abstract")]
    public Abstract Abstract { get; set; } = new();

    [XmlArray("supplemental_notes")]
    [XmlArrayItem("supplemental_note")]
    public List<string> SupplementalNotes { get; set; } = [];

    [XmlArray("index_terms")]
    [XmlArrayItem("term")]
    public List<Term> IndexTerms { get; set; } = [];

    [XmlArray("subject_areas")]
    [XmlArrayItem("subject_area")]
    public List<SubjectArea> SubjectAreas { get; set; } = [];

    [XmlElement(ElementName = "project")]
    public Project Project { get; set; } = new();

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
