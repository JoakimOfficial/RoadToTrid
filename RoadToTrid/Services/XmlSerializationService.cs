using RoadToTrid.Data.Interfaces;
using RoadToTrid.Data.Models.MarcXmlModels;
using RoadToTrid.Data.Models.TridXmlModels.Bibcat;
using RoadToTrid.Helpers;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace RoadToTrid.Services;

public class XmlSerializationService
{
    public MarcCollectionModel DeserializeXmlToCollection(string xmlContent)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(MarcCollectionModel));

        using StringReader reader = new(xmlContent);
        
        MarcCollectionModel collection = (MarcCollectionModel)serializer.Deserialize(reader);

        return collection;
    }
    public string SerializeRecordsToXml(IRecords records)
    {
        using var stringWriter = new StringWriter();

        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "  ",
            NewLineOnAttributes = false
        };

        // Define only the "xsi" namespace without "xsd"
        XmlSerializerNamespaces namespaces = new();
        namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

        // Serialize the object to XML
        XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);

        XmlSerializer serializer = new(records.GetType());
        serializer.Serialize(xmlWriter, records, namespaces);
        

        return stringWriter.ToString();
    }

    public byte[] SerializeRecordsToUtf8Xml(IRecords records)
    {
        var settings = new XmlWriterSettings
        {
            Indent = true,
            IndentChars = "  ",
            Encoding = Encoding.UTF8,
            NewLineOnAttributes = false
        };

        using var memoryStream = new MemoryStream();
        using var xmlWriter = XmlWriter.Create(memoryStream, settings);

        var serializer = new XmlSerializer(records.GetType());

        var namespaces = new XmlSerializerNamespaces();
        namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");

        serializer.Serialize(xmlWriter, records, namespaces);
        xmlWriter.Flush();

        return memoryStream.ToArray();
    }
}
