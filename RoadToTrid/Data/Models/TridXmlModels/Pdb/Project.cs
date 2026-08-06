using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Pdb;

[XmlRoot(ElementName = "project")]
public class Project
{
    [XmlElement(ElementName = "project_status")]
    public ProjectStatus ProjectStatus { get; set; }

    [XmlElement(ElementName = "funding")]
    public string Funding { get; set; }

    [XmlArray("deliverables")]
    [XmlArrayItem("deliverable")]
    public List<string> Deliverables { get; set; } = [];

    [XmlElement(ElementName = "notice_date")]
    public string NoticeDate { get; set; }

    [XmlElement(ElementName = "start_date")]
    public string StartDate { get; set; }

    [XmlElement(ElementName = "expected_completion_date")]
    public string ExpectedCompletionDate { get; set; }

    [XmlElement(ElementName = "actual_completion_date")]
    public string ActualCompletionDate { get; set; }

    [XmlArray("performing_agencies")]
    [XmlArrayItem("performing_agency")]
    public List<string> PerformingAgencies { get; set; } = [];

    [XmlArray("funding_agencies")]
    [XmlArrayItem("funding_agency")]
    public List<string> FundingAgencies { get; set; } = [];

    [XmlArray("responsible_individuals")]
    [XmlArrayItem("responsible_individual")]
    public List<ResponsibleIndividual> ResponsibleIndividuals { get; set; } = [];

    [XmlArray("investigators")]
    [XmlArrayItem("investigator")]
    public List<string> Investigators { get; set; } = [];

    [XmlArray("programs")]
    [XmlArrayItem("program")]
    public List<string> Programs { get; set; } = [];
}