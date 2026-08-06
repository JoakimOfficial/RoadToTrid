using RoadToTrid.Data.Enums;
using RoadToTrid.Data.Interfaces;
using RoadToTrid.Data.Models;
using RoadToTrid.Data.Models.MarcXmlModels;

namespace RoadToTrid.Services.Mapping;

public class MarcToTridMappingService
{
    private readonly Dictionary<DatabaseSource, IRecordMappingService> _mappers;

    public MarcToTridMappingService(IEnumerable<IRecordMappingService> services)
    {
        _mappers = services.ToDictionary(service => service.Source);
    }

    public IRecords Transform(MarcCollectionModel collection, DatabaseSource source, List<InvalidAbstractModel> invalids)
    {
        if (!_mappers.TryGetValue(source, out var mapper))
            throw new InvalidOperationException($"No mapper found for {source}");

        return mapper.MapAll(collection, invalids);
    }
}

