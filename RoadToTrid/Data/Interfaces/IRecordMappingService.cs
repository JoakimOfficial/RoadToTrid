using RoadToTrid.Data.Enums;
using RoadToTrid.Data.Models;
using RoadToTrid.Data.Models.MarcXmlModels;

namespace RoadToTrid.Data.Interfaces;

public interface IRecordMappingService
{
    DatabaseSource Source { get; }
    IRecords MapAll(MarcCollectionModel collection, List<InvalidAbstractModel> invalidAbstracts);
}
