using System.Xml.Serialization;

namespace RoadToTrid.Data.Models.MarcXmlModels;

[XmlRoot(ElementName = "record")]
public class MarcRecordModel
{
    [XmlElement(ElementName = "leader")]
    public string Leader { get; set; }

    [XmlElement(ElementName = "controlfield")]
    public List<MarcControlFieldModel> Controlfield { get; set; }

    [XmlElement(ElementName = "datafield")]
    public List<MarcDataFieldModel> Datafield { get; set; }
}