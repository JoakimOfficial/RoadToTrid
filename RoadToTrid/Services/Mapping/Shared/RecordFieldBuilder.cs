using RoadToTrid.Data.Constants;
using RoadToTrid.Data.Enums;
using RoadToTrid.Data.Mappings;
using RoadToTrid.Data.Models;
using RoadToTrid.Data.Models.MarcXmlModels;
using RoadToTrid.Data.Models.TridXmlModels.Shared;
using RoadToTrid.Helpers;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace RoadToTrid.Services.Mapping.Shared;

public class RecordFieldBuilder
{
    public static ValueTuple<Language1, Language2?, Language3?> CreateLanguages(List<MarcDataFieldModel> dataFieldEntries, List<MarcControlFieldModel> controlFieldEntries)
    {
        ValueTuple<Language1, Language2?, Language3?> languages = new(new Language1(), null, null);

        List<string> languageTexts = FieldProcessingHelper.GetSubfieldTextsByTag(dataFieldEntries, "041");

        if (languageTexts.Count == 0)
        {
            string somethingRandom = FieldProcessingHelper.GetControlFieldTextByTag(controlFieldEntries, "008");

            string languageCode = somethingRandom.Substring(35, 3); // Maybe create a helper function for this.

            languages.Item1.Id = "";
            languages.Item1.Code = languageCode;
            languages.Item1.Text = DictionaryHelper.GetValue(LanguageMappings.Languages, languageCode);

            return languages;
        }

        for (int i = 0; i < languageTexts.Count; i++)
        {
            if (i == 0)
            {
                languages.Item1.Id = "";
                languages.Item1.Code = languageTexts[0];
                languages.Item1.Text = DictionaryHelper.GetValue(LanguageMappings.Languages, languageTexts[0]);
            }

            if (i == 1)
            {
                languages.Item2 = new Language2()
                {
                    Id = "",
                    Code = languageTexts[1],
                    Text = DictionaryHelper.GetValue(LanguageMappings.Languages, languageTexts[1]),
                };
            }

            if (i == 2)
            {
                languages.Item3 = new Language3()
                {
                    Id = "",
                    Code = languageTexts[2],
                    Text = DictionaryHelper.GetValue(LanguageMappings.Languages, languageTexts[2]),
                };
            }
        }

        return languages;
    }

    public static List<Term> CreateIndexTerms(List<MarcDataFieldModel> dataFieldEntries, string tag)
    {
        List<string> termTexts = FieldProcessingHelper.GetSubfieldTextsByTag(dataFieldEntries, tag);

        List<Term> indexTerms = [];

        foreach (string termText in termTexts)
        {
            Term term = new()
            {
                Type = "EN",
                Code = DictionaryHelper.GetValue(ItrdTermMappings.ItrdTerms, termText),
                Text = termText,
            };

            if (!string.IsNullOrEmpty(term.Code))
            {
                indexTerms.Add(term);
            }
        }

        return indexTerms;
    }

    public static string CreateRecordType(List<MarcDataFieldModel> dataFieldEntries)
    {
        bool shouldBeLabeledAsRearchProject = FieldProcessingHelper.HasTag(dataFieldEntries, "651");

        if (shouldBeLabeledAsRearchProject)
        {
            return "Research project";
        }

        bool shouldBeLabeledAsComponent = FieldProcessingHelper.HasTag(dataFieldEntries, "773");

        if (shouldBeLabeledAsComponent)
        {
            return "Component";
        }
        else
        {
            return "Monograph";
        }
    }

    public static List<Url> CreateDocumentUrls(List<MarcDataFieldModel> dataFieldEntries, string tag)
    {
        //List<string> urlTexts = FieldProcessingHelper.GetSubfieldTextsByTag(DataFieldEntries, tag);

        //List <Url> documentUrls = [];

        //for (int i = 0; i < urlTexts.Count; i++)
        //{
        //    bool isUrl = urlTexts[i].Length >= 4 && urlTexts[i][..4] == "http";

        //    if (isUrl)
        //    {
        //        Url url = new()
        //        {
        //            Type = "D",
        //            Text = urlTexts[i]
        //        };

        //        documentUrls.Add(url);
        //    }
        //}

        List<MarcSubFieldModel> subfields = FieldProcessingHelper.GetSubfieldsByTag(dataFieldEntries, tag);

        MarcSubFieldModel? subfield = subfields.FirstOrDefault(sf => sf.Code == "u" && !sf.Text.Contains("https://projektdatabas.vti.se/"));

        List<Url> documentUrls = [];

        Url url = new()
        {
            Type = "D",
            Text = subfield?.Text ?? string.Empty,
        };

        documentUrls.Add(url);

        return documentUrls;
    }

    public static Abstract CreateAbstract(List<MarcDataFieldModel> dataFieldEntries, string tag, Language1 language1, List<InvalidAbstractModel> invalidAbstracts)
    {
        List<string> abstractTexts = FieldProcessingHelper.GetSubfieldTextsByTag(dataFieldEntries, tag);

        string abstractText = string.Empty;

        if (abstractTexts.Count == 2)
        {
            int expectedEnglishIndex = language1.Code == "eng" ? 0 : 1;
            int? detectedEnglishIndex = DetectLikelyEnglishAbstractIndex(abstractTexts);

            if (detectedEnglishIndex.HasValue)
            {
                abstractText = abstractTexts[detectedEnglishIndex.Value];

                if (detectedEnglishIndex.Value != expectedEnglishIndex)
                {
                    invalidAbstracts.Add(new InvalidAbstractModel
                    {
                        AbstractText = abstractText,
                        RecordNumber = CreateRecordNoAttribute(dataFieldEntries),
                        WarningType = AbstractWarningType.LanguageMismatch,
                        ExpectedLanguageCode = "eng",
                        DetectedLanguageCode = "eng",
                        ExpectedAbstractNumber = expectedEnglishIndex + 1,
                        DetectedAbstractNumber = detectedEnglishIndex.Value + 1,
                    });
                }
            }
            else
            {
                abstractText = abstractTexts[expectedEnglishIndex];
            }
        }
        else if (abstractTexts.Count == 1 && language1.Code == "eng")
        {
            abstractText = abstractTexts[0];
        }

        // Proper Unicode-safe check for illegal characters
        List<int> illegalIndexes = new();

        if(!string.IsNullOrEmpty(abstractText))
        {
            var textEnum = StringInfo.GetTextElementEnumerator(abstractText);
            int elementIndex = 0;

            while (textEnum.MoveNext())
            {
                string textElement = textEnum.GetTextElement();

                if (CharacterMappings.Characters.ContainsKey(textElement))
                {
                    elementIndex++;
                    continue;
                }

                if (CharacterConstants.MathStyledChars.Contains(textElement))
                {
                    illegalIndexes.Add(elementIndex);
                }

                elementIndex++;
            }
        }

        if (illegalIndexes.Count > 0 || string.IsNullOrEmpty(abstractText))
        {
            InvalidAbstractModel invalidAbstract = new()
            {
                Indexes = illegalIndexes,
                AbstractText = abstractText,
                RecordNumber = CreateRecordNoAttribute(dataFieldEntries),
                WarningType = string.IsNullOrEmpty(abstractText)
                    ? AbstractWarningType.MissingEnglishAbstract
                    : AbstractWarningType.RemovedInvalidCharacters,
            };

            invalidAbstracts.Add(invalidAbstract);

            abstractText = string.Empty;
        }

        Abstract englishAbstract = new()
        {
            Original = string.Empty,
            Text = StringHelper.EncodeSpecialCharsForXml(abstractText).Trim(),
        };

        return englishAbstract;
    }

    private static int? DetectLikelyEnglishAbstractIndex(List<string> abstractTexts)
    {
        if (abstractTexts.Count != 2)
        {
            return null;
        }

        int firstScore = ScoreEnglishLikelihood(abstractTexts[0]);
        int secondScore = ScoreEnglishLikelihood(abstractTexts[1]);

        if (Math.Abs(firstScore - secondScore) < 3)
        {
            return null;
        }

        return firstScore > secondScore ? 0 : 1;
    }

    private static int ScoreEnglishLikelihood(string text)
    {
        string normalized = text.ToLowerInvariant();
        string[] tokens = Regex.Split(normalized, @"[^\p{L}]+")
            .Where(token => token.Length > 0)
            .ToArray();

        HashSet<string> englishWords =
        [
            "the", "and", "of", "to", "in", "for", "with", "that", "this", "is",
            "are", "as", "on", "by", "from", "an", "be", "it", "or", "which",
        ];

        HashSet<string> swedishWords =
        [
            "och", "att", "det", "som", "en", "ett", "är", "för", "med", "av",
            "på", "till", "i", "om", "den", "de", "har", "kan", "där", "samt",
        ];

        int englishScore = tokens.Count(englishWords.Contains);
        int swedishScore = tokens.Count(swedishWords.Contains);

        if (normalized.Any(character => character is 'å' or 'ä' or 'ö'))
        {
            swedishScore += 4;
        }

        return englishScore - swedishScore;
    }

    public static List<SubjectArea> CreateSubjectAreas(List<MarcDataFieldModel> dataFieldEntries, string tag)
    {
        List<string> subjectAreaTexts = FieldProcessingHelper.GetSubfieldTextsByTag(dataFieldEntries, tag);

        List<SubjectArea> subjectAreas = [];

        foreach (string subjectAreaText in subjectAreaTexts)
        {
            SubjectArea subjectArea = new()
            {
                Code = subjectAreaText,
                Text = DictionaryHelper.GetValue(ItrdSubjectMappings.ItrdSubjects, subjectAreaText),
            };

            if (!string.IsNullOrEmpty(subjectArea.Text))
            {
                subjectAreas.Add(subjectArea);
            }

        }

        return subjectAreas;
    }

    public static string CreateRecordNoAttribute(List<MarcDataFieldModel> dataFieldEntries)
    {
        return FieldProcessingHelper.GetSingleSubfieldTextByTag(dataFieldEntries, "999");
    }

    public static List<TrisFile> CreateTrisFiles()
    {
        List<TrisFile> trisFiles = [];

        trisFiles.Add(new TrisFile()
        {
            Code = TrisFileConstants.VALUE_1.ToLower(),
            Text = TrisFileConstants.VALUE_1,
        });

        trisFiles.Add(new TrisFile()
        {
            Code = TrisFileConstants.VALUE_2.ToLower(),
            Text = TrisFileConstants.VALUE_2,
        });

        return trisFiles;
    }
}
