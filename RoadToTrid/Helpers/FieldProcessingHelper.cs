using RoadToTrid.Data.Models.MarcXmlModels;

namespace RoadToTrid.Helpers;

public static class FieldProcessingHelper
{
    public static List<MarcDataFieldModel> GetDataFieldEntries(MarcRecordModel record)
    {
        return record.Datafield
            .GroupBy(df => df.Tag)
            .Select(group => new MarcDataFieldModel
            {
                Tag = group.Key,
                Subfields = group.SelectMany(df => df.Subfields.Select(sf => new MarcSubFieldModel
                {
                    Code = sf.Code,
                    Text = sf.Text,
                })).ToList()
            })
            .ToList();
    }

    public static List<MarcControlFieldModel> GetControlFieldEntries(MarcRecordModel record)
    {
        return record.Controlfield
            .Select(cf => new MarcControlFieldModel
            {
                Tag = cf.Tag,
                Text = cf.Text,
            })
            .ToList();
    }

    public static List<string?> GetSubfieldTextsByTag(List<MarcDataFieldModel> groupedData, string tag)
    {
        // Find the DataFieldEntryModel with the matching tag
        MarcDataFieldModel? result = groupedData.FirstOrDefault(group => group.Tag == tag);

        // Check if result is null before accessing Subfields
        if (result == null || result.Subfields.Count == 0)
        {
            return new List<string?>(); // Return an empty list if no matching tag is found
        }

        // Extract the Subfield Texts
        List<string?> texts = result.Subfields.Select(sf => sf.Text).ToList();

        return texts;
    }

    public static List<MarcSubFieldModel> GetSubfieldsByTag(List<MarcDataFieldModel> groupedData, string tag)
    {
        // Find the matching group by tag
        MarcDataFieldModel? result = groupedData.FirstOrDefault(group => group.Tag == tag);

        // Check if result is null and return an empty list if not found, otherwise return the Subfields
        return result != null && result.Subfields.Count > 0 ? result.Subfields : new List<MarcSubFieldModel>();
    }

    public static List<MarcDataFieldModel> GetDataFieldsByTag(List<MarcDataFieldModel> groupedData, string tag)
    {
        List<MarcDataFieldModel> result = groupedData.Where(group => group.Tag == tag).ToList();
        // Use Where to find all DataFieldEntryModel objects that match the tag and convert it to a list
        return result;
    }

    public static string GetSingleSubfieldTextByTag(List<MarcDataFieldModel> groupedData, string tag)
    {
        MarcDataFieldModel? result = groupedData.FirstOrDefault(group => group.Tag == tag);
        return result != null ? result.Subfields[0].Text : string.Empty;
    }

    public static string GetSingleSubfieldTextByCode(List<MarcSubFieldModel> groupedData, string code)
    {
        MarcSubFieldModel? result = groupedData.FirstOrDefault(group => group.Code == code);
        return result != null ? result.Text : string.Empty;
    }

    // Rename to something with controlfields
    public static string GetControlFieldTextByTag(List<MarcControlFieldModel> groupedData, string tag)
    {
        MarcControlFieldModel? result = groupedData.FirstOrDefault(group => group.Tag == tag);
        return result.Text;
    }

    public static bool HasTag(List<MarcDataFieldModel> groupedData, string tag)
    {
        MarcDataFieldModel? result = groupedData.FirstOrDefault(group => group.Tag == tag);
        return result != null;
    }
}
