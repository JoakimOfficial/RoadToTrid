using RoadToTrid.Data.Enums;
using RoadToTrid.Data.Interfaces;
using RoadToTrid.Data.Models;
using RoadToTrid.Data.Models.MarcXmlModels;
using RoadToTrid.Data.Models.TridXmlModels.Pdb;
using RoadToTrid.Data.Models.TridXmlModels.Shared;
using RoadToTrid.Helpers;
using RoadToTrid.Services.Mapping.Shared;

namespace RoadToTrid.Services.Mapping.Pdb;

public class PdbRecordMappingService : IRecordMappingService
{
    public DatabaseSource Source => DatabaseSource.Projektdatabasen;

    public IRecords MapAll(MarcCollectionModel collection, List<InvalidAbstractModel> invalidAbstracts)
    {
        PdbRecords pdbRecords = new();

        foreach (MarcRecordModel marcRecord in collection.Records)
        {
            List<MarcDataFieldModel> dataFieldEntries = FieldProcessingHelper.GetDataFieldEntries(marcRecord);
            List<MarcControlFieldModel> controlFieldEntries = FieldProcessingHelper.GetControlFieldEntries(marcRecord);

            (Language1, Language2?, Language3?) languages = RecordFieldBuilder.CreateLanguages(dataFieldEntries, controlFieldEntries);

            PdbRecord pdbRecord = new()
            {
                Title = PdbFieldBuilder.CreateTitle(dataFieldEntries),
                ForeignTitle = PdbFieldBuilder.CreateForeignTitle(dataFieldEntries),
                Project = PdbFieldBuilder.CreateProject(dataFieldEntries),

                RecordNo = RecordFieldBuilder.CreateRecordNoAttribute(dataFieldEntries),
                RecordType = RecordFieldBuilder.CreateRecordType(dataFieldEntries),
                DocumentUrls = RecordFieldBuilder.CreateDocumentUrls(dataFieldEntries, "856"),
                TrisFiles = RecordFieldBuilder.CreateTrisFiles(),
                IndexTerms = RecordFieldBuilder.CreateIndexTerms(dataFieldEntries, "650"),
                SubjectAreas = RecordFieldBuilder.CreateSubjectAreas(dataFieldEntries, "945"),
                Abstract = RecordFieldBuilder.CreateAbstract(dataFieldEntries, "520", languages.Item1, invalidAbstracts),
                Language1 = languages.Item1,

                // Is this needed?
                //Attributes = null,
            };

            pdbRecords.Records.Add(pdbRecord);
        }

        return pdbRecords;
    }
}
