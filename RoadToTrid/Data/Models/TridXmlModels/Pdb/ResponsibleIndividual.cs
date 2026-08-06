using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.TridXmlModels.Pdb;

public class ResponsibleIndividual
{
    [XmlAttribute(AttributeName = "lastname")]
    public string Lastname { get; set; }

    [XmlAttribute(AttributeName = "firstname")]
    public string Firstname { get; set; }
}