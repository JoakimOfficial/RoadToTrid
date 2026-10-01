using RoadToTrid.Data.Enums;
using RoadToTrid.Data.Interfaces;
using RoadToTrid.Data.Models;
using RoadToTrid.Data.Models.MarcXmlModels;
using RoadToTrid.Data.Models.TridXmlModels.Bibcat;
using RoadToTrid.Data.Models.TridXmlModels.Shared;
using RoadToTrid.Helpers;
using RoadToTrid.Services.Mapping.Shared;

namespace RoadToTrid.Services.Mapping.Bibcat;

public class BibcatRecordMappingService : IRecordMappingService
{
    private readonly ApplicationSettingsService _applicationSettingsService;

    public BibcatRecordMappingService(ApplicationSettingsService applicationSettingsService)
    {
        _applicationSettingsService = applicationSettingsService;
    }

    public DatabaseSource Source => DatabaseSource.Bibliotekskatalogen;

    public IRecords MapAll(MarcCollectionModel collection, List<InvalidAbstractModel> invalidAbstracts)
    {
        BibcatRecords bibcatRecords = new();
        AvailabilityAgency availabilityAgency = _applicationSettingsService.GetAvailabilityAgency();

        foreach (MarcRecordModel marcRecord in collection.Records)
        {
            List<MarcDataFieldModel> dataFieldEntries = FieldProcessingHelper.GetDataFieldEntries(marcRecord);
            List<MarcControlFieldModel> controlFieldEntries = FieldProcessingHelper.GetControlFieldEntries(marcRecord);

            (Language1, Language2?, Language3?) languages = RecordFieldBuilder.CreateLanguages(dataFieldEntries, controlFieldEntries);

            BibcatRecord bibcatRecord = new()
            {
                Title = BibcatFieldBuilder.CreateTitle(dataFieldEntries, "245"),
                ForeignTitle = BibcatFieldBuilder.CreateForeignTitle(dataFieldEntries, "246"),
                Document = BibcatFieldBuilder.CreateDocument(dataFieldEntries, "007", availabilityAgency),

                RecordNo = RecordFieldBuilder.CreateRecordNoAttribute(dataFieldEntries),
                RecordType = RecordFieldBuilder.CreateRecordType(dataFieldEntries),
                DocumentUrls = RecordFieldBuilder.CreateDocumentUrls(dataFieldEntries, "856"),
                TrisFiles = RecordFieldBuilder.CreateTrisFiles(),
                IndexTerms = RecordFieldBuilder.CreateIndexTerms(dataFieldEntries, "650"),
                SubjectAreas = RecordFieldBuilder.CreateSubjectAreas(dataFieldEntries, "945"),
                Abstract = RecordFieldBuilder.CreateAbstract(dataFieldEntries, "520", languages.Item1, invalidAbstracts),
                Language1 = languages.Item1,
                Language2 = languages.Item2,
                Language3 = languages.Item3,
            };

            bibcatRecords.Records.Add(bibcatRecord);
        }

        return bibcatRecords;
    }
}
