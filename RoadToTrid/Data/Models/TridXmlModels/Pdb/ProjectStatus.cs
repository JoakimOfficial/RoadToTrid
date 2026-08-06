using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Pdb;

public class ProjectStatus
{
    [XmlAttribute(AttributeName = "code")]
    public string Code { get; set; }

    [XmlText]
    public string Text { get; set; }
}