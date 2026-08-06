using RoadToTrid.Data.Models.MarcXmlModels;
using RoadToTrid.Data.Models.TridXmlModels.Pdb;
using RoadToTrid.Helpers;

namespace RoadToTrid.Services.Mapping.Pdb;

public class PdbFieldBuilder
{
    public static string CreateTitle(List<MarcDataFieldModel> dataFieldEntries, string tag = "245")
    {
        List<string> titleTexts = FieldProcessingHelper.GetSubfieldTextsByTag(dataFieldEntries, tag);

        if (titleTexts.Count == 0)
        {
            return null;
        }

        return StringHelper.EncodeSpecialCharsForXml(titleTexts[0].Trim());
    }

    public static string CreateForeignTitle(List<MarcDataFieldModel> dataFieldEntries, string tag = "245")
    {
        List<string> titleTexts = FieldProcessingHelper.GetSubfieldTextsByTag(dataFieldEntries, tag);

        if (titleTexts.Count != 2)
        {
            return null;
        }

        string foreignTitle = StringHelper.RemoveSurroundingParentheses(titleTexts[1].Trim());

        return StringHelper.EncodeSpecialCharsForXml(foreignTitle);
    }

    public static Project CreateProject(List<MarcDataFieldModel> dataFieldEntries)
    {
        Project project = new();

        project.ProjectStatus = null; //CreateProjectStatus();
        project.Funding = String.Empty;
        project.NoticeDate = String.Empty;
        project.StartDate = CreateDateByType(dataFieldEntries, "startDate");
        project.ExpectedCompletionDate = CreateDateByType(dataFieldEntries, "ExpectedCompletionDate");
        project.ActualCompletionDate = String.Empty;
        project.PerformingAgencies = CreatePerformingAgencies(dataFieldEntries);
        project.ResponsibleIndividuals = CreateResponsibleIndividuals(dataFieldEntries);

        return project;
    }

    public static string CreateDateByType(List<MarcDataFieldModel> dataFieldEntries, string type)
    {
        List<MarcSubFieldModel> matchingSubFieldEntries = FieldProcessingHelper.GetSubfieldsByTag(dataFieldEntries, "260");

        string dateString = FieldProcessingHelper.GetSingleSubfieldTextByCode(matchingSubFieldEntries, "c");

        return StringHelper.GetDatePart(dateString, type);
    }

    public static List<string>? CreatePerformingAgencies(List<MarcDataFieldModel> dataFieldEntries)
    {
        List<MarcSubFieldModel> matchingSubFieldEntries = FieldProcessingHelper.GetSubfieldsByTag(dataFieldEntries, "110");

        if (matchingSubFieldEntries.Count == 0)
        {
            return null;
        }

        List<string> performingAgencies = [];

        foreach (MarcSubFieldModel subfieldEntry in matchingSubFieldEntries)
        {
            if (subfieldEntry.Text != null)
            {
                performingAgencies.Add(subfieldEntry.Text);
            }
        }
        ;

        return performingAgencies;
    }

    public static List<ResponsibleIndividual> CreateResponsibleIndividuals(List<MarcDataFieldModel> dataFieldEntries)
    {
        List<ResponsibleIndividual> responsibleIndividuals = [];

        List<MarcSubFieldModel> matchingSubFieldEntries = FieldProcessingHelper.GetSubfieldsByTag(dataFieldEntries, "100");

        matchingSubFieldEntries.AddRange(FieldProcessingHelper.GetSubfieldsByTag(dataFieldEntries, "700"));

        foreach (MarcSubFieldModel subfieldEntry in matchingSubFieldEntries)
        {
            ResponsibleIndividual responsibleIndividual = new();

            if (subfieldEntry.Code == "a" && subfieldEntry.Text != null)
            {
                string[] nameSegments = subfieldEntry.Text.Split(",");

                responsibleIndividual.Lastname = nameSegments[0].Trim();
                responsibleIndividual.Firstname = nameSegments[1].Trim();
            }

            responsibleIndividuals.Add(responsibleIndividual);
        }

        return responsibleIndividuals;
    }
}
