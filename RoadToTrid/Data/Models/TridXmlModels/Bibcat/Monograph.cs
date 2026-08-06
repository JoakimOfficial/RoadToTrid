using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Bibcat;

[XmlRoot(ElementName = "monograph")]
public class Monograph
{
    [XmlArray("corporate_authors")]
    [XmlArrayItem("corporate_author")]
    public List<string>? CorporateAuthors { get; set; }

    [XmlArray("editors")]
    [XmlArrayItem("editor")]
    public List<string> Editors { get; set; } = [];

    [XmlElement(ElementName = "serial")]
    public Serial Serial { get; set; }

    [XmlElement(ElementName = "isbn")]
    public string? Isbn { get; set; }

    [XmlElement(ElementName = "publication_date")]
    public PublicationDate PublicationDate { get; set; } = new();

    [XmlElement(ElementName = "issue")]
    public string? Issue { get; set; }

    [XmlArray("availability_agencies")]
    [XmlArrayItem("availability_agency")]
    public List<AvailabilityAgency> AvailabilityAgencies { get; set; } = [];

    [XmlAttribute(AttributeName = "title")]
    public string Title { get; set; }

    [XmlAttribute(AttributeName = "accession_number")]
    public string AccessionNumber { get; set; } = "0";

    [XmlText]
    public string Text { get; set; }
}
