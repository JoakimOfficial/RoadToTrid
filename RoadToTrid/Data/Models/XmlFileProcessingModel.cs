using RoadToTrid.Data.Enums;
using RoadToTrid.Data.Models.MarcXmlModels;
using RoadToTrid.Data.Models.TridXmlModels.Bibcat;
using RoadToTrid.Data.Models.TridXmlModels.Pdb;
using System.Xml.Linq;

namespace RoadToTrid.Data.Models;

public class XmlFileProcessingModel
{
    public IFormFile? InputFile { get; set; }
    public XDocument? ParsedXmlDocument { get; set; }
    public DatabaseSource Source { get; set; }
    public MarcCollectionModel? DeserializedMarcCollection { get; set; }
    public BibcatRecords? ConvertedBibcatTridRecords { get; set; } = new();
    public PdbRecords? ConvertedPdbTridRecords { get; set; } = new();
    public string? SerializedTridRecordsXml { get; set; }
    public byte[] OutPutXmlFileBytes { get; set; } = [];
    public string? InputFileName { get; set; }
    public string? OutputFileName { get; set; }
    public long? InputFileSize { get; set; }
    public long? OutputFileSize { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ErrorCode { get; set; }
    public bool IsValid { get; set; } = false;
    public FileProcessingStatus Status { get; set; } = FileProcessingStatus.Waiting;
    public DateTime? LoadedTime { get; set; }
    public DateTime? ProcessedTime { get; set; }

    public List<InvalidAbstractModel> InvalidAbstracts { get; set; } = [];
    public List<TextQualityWarningModel> TextQualityWarnings { get; set; } = [];
}
