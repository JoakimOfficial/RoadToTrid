using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Bibcat;

[XmlRoot(ElementName = "author")]
public class Author
{
    [XmlAttribute(AttributeName = "firstname")]
    public string Firstname { get; set; }

    [XmlAttribute(AttributeName = "middlename")]
    public string Middlename { get; set; }

    [XmlAttribute(AttributeName = "lastname")]
    public string Lastname { get; set; }

    [XmlAttribute(AttributeName = "suffix")]
    public string Suffix { get; set; }

    [XmlAttribute(AttributeName = "phone")]
    public string Phone { get; set; }

    [XmlAttribute(AttributeName = "fax")]
    public string Fax { get; set; }

    [XmlAttribute(AttributeName = "email")]
    public string Email { get; set; }

    [XmlAttribute(AttributeName = "agency_affiliation")]
    public string AgencyAffiliation { get; set; }

    [XmlAttribute(AttributeName = "position")]
    public string Position { get; set; }

    [XmlText]
    public string Text { get; set; }
}