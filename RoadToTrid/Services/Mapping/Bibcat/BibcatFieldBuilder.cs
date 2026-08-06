using RoadToTrid.Data.Models.MarcXmlModels;
using RoadToTrid.Data.Models.TridXmlModels.Bibcat;
using RoadToTrid.Helpers;

namespace RoadToTrid.Services.Mapping.Bibcat;

public class BibcatFieldBuilder
{
    public static string CreateTitle(List<MarcDataFieldModel> dataFieldEntries, string tag = "245")
    {
        List<string> titleTexts = FieldProcessingHelper.GetSubfieldTextsByTag(dataFieldEntries, "245");

        if (titleTexts.Count == 0)
        {
            return null;
        }

        if (titleTexts.Count == 2)
        {
            return StringHelper.EncodeSpecialCharsForXml(titleTexts[0].Trim() + " " + titleTexts[1].Trim());
        }

        return StringHelper.EncodeSpecialCharsForXml(titleTexts[0].Trim());
    }
    public static string CreateForeignTitle(List<MarcDataFieldModel> dataFieldEntries, string tag)
    {
        List<string> titleTexts = FieldProcessingHelper.GetSubfieldTextsByTag(dataFieldEntries, "246");

        if (titleTexts.Count == 0)
        {
            return null;
        }

        // Returns Title if tag="245" and Foreign Title when tag="246"
        if (titleTexts.Count == 2)
        {
            return StringHelper.EncodeSpecialCharsForXml(titleTexts[0].Trim() + " " + titleTexts[1].Trim());
        }

        return StringHelper.EncodeSpecialCharsForXml(titleTexts[0].Trim());
    }

    public static Document CreateDocument(List<MarcDataFieldModel> dataFieldEntries, string tag)
    {
        Document document = new();

        document.MediaType = StringHelper.GetFirstCharacter(FieldProcessingHelper.GetSingleSubfieldTextByTag(dataFieldEntries, "007"));
        document.Pagination = StringHelper.ConvertSwedishPaginationToEnglish(FieldProcessingHelper.GetSingleSubfieldTextByTag(dataFieldEntries, "300"));
        document.Authors = CreateAuthors(dataFieldEntries);
        document.Monograph = CreateMonograph(dataFieldEntries);

        return document;
    }

    public static List<Author> CreateAuthors(List<MarcDataFieldModel> dataFieldEntries)
    {
        List<Author> authors = [];

        List<MarcSubFieldModel> matchingSubFieldEntries = FieldProcessingHelper.GetSubfieldsByTag(dataFieldEntries, "100");

        matchingSubFieldEntries.AddRange(FieldProcessingHelper.GetSubfieldsByTag(dataFieldEntries, "700"));

        foreach (MarcSubFieldModel subfieldEntry in matchingSubFieldEntries)
        {
            Author author = new();

            if (subfieldEntry.Code == "a" && subfieldEntry.Text != null)
            {
                string[] nameSegments = subfieldEntry.Text.Split(",");

                author.Firstname = nameSegments[1].Trim();
                author.Lastname = nameSegments[0].Trim();

                author.Text = $"{nameSegments[0].Trim()}, {nameSegments[1].Trim()}";
            }

            authors.Add(author);
        }

        return authors;
    }

    public static Monograph CreateMonograph(List<MarcDataFieldModel> dataFieldEntries)
    {
        Monograph monograph = new();

        monograph.CorporateAuthors = CreateCorporateAuthors(dataFieldEntries);
        monograph.Editors = [];
        monograph.Serial = CreateSerial(dataFieldEntries);
        monograph.Isbn = CreateIsbn(dataFieldEntries);
        monograph.PublicationDate = CreatePublicationDate(dataFieldEntries);
        monograph.Issue = CreateIssue(dataFieldEntries);
        monograph.AvailabilityAgencies = CreateAvailabilityAgencies();

        monograph.Title = "";
        monograph.AccessionNumber = "";
        monograph.Text = "";

        return monograph;
    }

    public static PublicationDate CreatePublicationDate(List<MarcDataFieldModel> dataFieldEntries)
    {
        PublicationDate publicationDate = new();

        List<MarcSubFieldModel> matchingSubFieldEntries = FieldProcessingHelper.GetSubfieldsByTag(dataFieldEntries, "260");

        string publicationYear = FieldProcessingHelper.GetSingleSubfieldTextByCode(matchingSubFieldEntries, "c");

        if (!string.IsNullOrEmpty(publicationYear))
        {
            publicationDate.Year = StringHelper.RemoveBracketsFromYear(publicationYear);
            publicationDate.Text = StringHelper.RemoveBracketsFromYear(publicationYear) + "0000";
        }
        else
        {
            string weirdText = FieldProcessingHelper.GetSingleSubfieldTextByTag(dataFieldEntries, "008");


        }


        // Add fallback to check for year inside 008 (weird string) if its missing in 260$c.

        return publicationDate;
    }

    public static string? CreateIsbn(List<MarcDataFieldModel> dataFieldEntries)
    {
        string isbn = FieldProcessingHelper.GetSingleSubfieldTextByTag(dataFieldEntries, "020");

        if (string.IsNullOrEmpty(isbn))
        {
            return null;
        }

        return isbn.Trim();
    }

    public static string? CreateIssue(List<MarcDataFieldModel> dataFieldEntries)
    {
        List<MarcSubFieldModel> matchingSubFieldEntries = FieldProcessingHelper.GetSubfieldsByTag(dataFieldEntries, "490");

        string issue = FieldProcessingHelper.GetSingleSubfieldTextByCode(matchingSubFieldEntries, "v");

        if (string.IsNullOrEmpty(issue))
        {
            return null;
        }

        return issue;
    }

    public static List<AvailabilityAgency> CreateAvailabilityAgencies()
    {
        List<AvailabilityAgency> availabilityAgencies = [];

        availabilityAgencies.Add(new AvailabilityAgency());

        return availabilityAgencies;
    }

    public static Serial? CreateSerial(List<MarcDataFieldModel> dataFieldEntries)
    {
        List<MarcSubFieldModel> matchingSubFieldEntries = FieldProcessingHelper.GetSubfieldsByTag(dataFieldEntries, "490");

        if (matchingSubFieldEntries.Count == 0)
        {
            return null;
        }

        List<MarcSubFieldModel> subfieldEntries2 = FieldProcessingHelper.GetSubfieldsByTag(dataFieldEntries, "022");

        string text = FieldProcessingHelper.GetSingleSubfieldTextByCode(matchingSubFieldEntries, "a");
        string issn = FieldProcessingHelper.GetSingleSubfieldTextByCode(matchingSubFieldEntries, "x");

        if (string.IsNullOrEmpty(issn))
        {
            issn = FieldProcessingHelper.GetSingleSubfieldTextByCode(subfieldEntries2, "a");
        }

        Serial serial = new Serial
        {
            Issn = issn,
            Text = text,
        };

        return serial;
    }

    public static List<string>? CreateCorporateAuthors(List<MarcDataFieldModel> dataFieldEntries)
    {
        List<MarcSubFieldModel> matchingSubFieldEntries = FieldProcessingHelper.GetSubfieldsByTag(dataFieldEntries, "110");

        if (matchingSubFieldEntries.Count == 0)
        {
            return null;
        }

        List<string> corporateAuthors = [];

        foreach (MarcSubFieldModel subfieldEntry in matchingSubFieldEntries)
        {
            if (subfieldEntry.Text != null)
            {
                //CorporateAuthor author = new()
                //{
                //    Text = subfieldEntry.Text,
                //};

                corporateAuthors.Add(subfieldEntry.Text);
            }
        };
        
        return corporateAuthors;
    }
}
