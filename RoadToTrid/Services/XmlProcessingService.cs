using RoadToTrid.Data.Enums;
using RoadToTrid.Data.Interfaces;
using RoadToTrid.Data.Models;
using RoadToTrid.Data.Models.TridXmlModels.Bibcat;
using RoadToTrid.Data.Models.TridXmlModels.Pdb;
using RoadToTrid.Helpers;
using RoadToTrid.Services.Mapping;

namespace RoadToTrid.Services;

public class XmlProcessingService
{
    private readonly XmlFileService _fileService;
    private readonly XmlSerializationService _serializationService;
    private readonly MarcToTridMappingService _mappingService;
    private readonly TextQualityReviewService _textQualityReviewService;

    public List<XmlFileProcessingModel> LoadedFiles { get; set; } = [];

    public bool ShowInvalidFileModal { get; set; } = false;

    public XmlProcessingService(XmlFileService fileService, XmlSerializationService serializationService, MarcToTridMappingService mappingService, TextQualityReviewService textQualityReviewService)
    {
        _fileService = fileService;
        _serializationService = serializationService;
        _mappingService = mappingService;
        _textQualityReviewService = textQualityReviewService;
    }

    public void LoadMultipleFiles(IReadOnlyCollection<IFormFile> files)
    {
        LoadedFiles.Clear();
        ShowInvalidFileModal = false;

        foreach (IFormFile file in files)
        {
            if (!file.FileName.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                ShowInvalidFileModal = true;
                LoadedFiles.Clear();
                return;
            }

            LoadedFiles.Add(_fileService.LoadXmlFile(file));
        }
    }

    public async Task ProcessXmlFile(XmlFileProcessingModel file)
    {
        file.Status = FileProcessingStatus.Processing;
        file.ParsedXmlDocument = await _fileService.ParseXmlFileToXdocumentAsync(file.InputFile, file);

        if (file.ParsedXmlDocument == null)
        {
            file.Status = FileProcessingStatus.Failed;
            return;
        }

        file.IsValid = await _fileService.ValidateXmlDocumentAgainstSchema(file.ParsedXmlDocument, file);

        if (file.IsValid == false)
        {
            file.Status = FileProcessingStatus.Failed;
            return;
        }

        string xmlSafeString = StringHelper.CleanStringForXml(file.ParsedXmlDocument.ToString());

        file.DeserializedMarcCollection = _serializationService.DeserializeXmlToCollection(xmlSafeString);

        file.Source = file.DeserializedMarcCollection.Records
            .FirstOrDefault()?
            .Datafield
            .Any(df => df.Tag == "651") == true ? DatabaseSource.Projektdatabasen : DatabaseSource.Bibliotekskatalogen;

        IRecords tridRecords = _mappingService.Transform(file.DeserializedMarcCollection, file.Source, file.InvalidAbstracts);

        if (tridRecords is BibcatRecords bibcatRecords)
        {
            await _textQualityReviewService.ReviewBibcatRecordsAsync(bibcatRecords, file.TextQualityWarnings);
        }

        SetConvertedTridRecordsOnModel(file, tridRecords);

        file.OutPutXmlFileBytes = _serializationService.SerializeRecordsToUtf8Xml(tridRecords);
        file.OutputFileSize = file.OutPutXmlFileBytes.Length / 1024;
        file.Status = FileProcessingStatus.Done;
    }

    private void SetConvertedTridRecordsOnModel(XmlFileProcessingModel file, IRecords convertedTridRecords)
    {
        if (convertedTridRecords is BibcatRecords bibcat)
        {
            file.ConvertedBibcatTridRecords = bibcat;
        }
        else if (convertedTridRecords is PdbRecords pdb)
        {
            file.ConvertedPdbTridRecords = pdb;
        }
        else
        {
            throw new InvalidOperationException("The provided records are of an unknown type.");
        }
    }
}
