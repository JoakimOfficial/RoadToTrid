namespace RoadToTrid.Data.Models.TridXmlModels.Pdb;

using System.Xml.Serialization;

[XmlRoot(ElementName = "attribute")]
public class Attribute
{
    [XmlText]
    public string Text { get; set; } = string.Empty;
}
