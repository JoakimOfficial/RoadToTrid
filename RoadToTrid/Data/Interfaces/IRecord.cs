using RoadToTrid.Data.Models.TridXmlModels.Shared;

namespace RoadToTrid.Data.Interfaces;

public interface IRecord
{
    public string Type { get; set; }
    public string AccessionNumber { get; set; }
    public string RecordType { get; set; }
    public string RecordStatus { get; set; }
    public string Title { get; set; }
    public string ForeignTitle { get; set; }
    public Language1 Language1 { get; set; }
    public Abstract Abstract { get; set; }
    public List<SubjectArea> SubjectAreas { get; set; }
    public List<Url> DocumentUrls { get; set; }
    public List<Term> IndexTerms { get; set; }
    public List<TrisFile> TrisFiles { get; set; }
}
