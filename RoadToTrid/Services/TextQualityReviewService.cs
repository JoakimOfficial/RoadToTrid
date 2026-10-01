using RoadToTrid.Data.Enums;
using RoadToTrid.Data.Models;
using RoadToTrid.Data.Models.TridXmlModels.Bibcat;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace RoadToTrid.Services;

public class TextQualityReviewService
{
    private readonly ApplicationSettingsService _applicationSettingsService;
    private readonly HttpClient _httpClient;
    private static readonly Regex SuspiciousSplitWordPattern = new(@"\b(?:[A-Za-z]{2,}\s+[A-Za-z]{1,3}|[A-Za-z]{1,3}\s+[A-Za-z]{2,})\b", RegexOptions.Compiled);
    private static readonly Regex UnknownCharacterMarkerPattern = new(@"(<\?>|&lt;\?&gt;|�|\uFFFD)", RegexOptions.Compiled);
    private static readonly HashSet<string> AllowedShortWords =
    [
        "a", "an", "as", "at", "be", "by", "do", "go", "he", "if", "in", "is", "it",
        "me", "my", "no", "of", "on", "or", "so", "to", "up", "us", "we",
        "and", "are", "but", "can", "did", "for", "has", "had", "her", "him", "his",
        "its", "may", "not", "our", "the", "was", "way", "who", "you",
    ];
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };

    public TextQualityReviewService(ApplicationSettingsService applicationSettingsService, HttpClient httpClient)
    {
        _applicationSettingsService = applicationSettingsService;
        _httpClient = httpClient;
    }

    public async Task ReviewBibcatRecordsAsync(BibcatRecords records, List<TextQualityWarningModel> warnings)
    {
        TextQualityReviewSettings settings = _applicationSettingsService.GetTextQualityReviewSettings();

        if (!settings.IsEnabled)
        {
            return;
        }

        if (!Uri.TryCreate(settings.OllamaUrl, UriKind.Absolute, out Uri? baseUri))
        {
            warnings.Add(new TextQualityWarningModel
            {
                Level = TextQualityWarningLevel.Information,
                Message = "AI text check is enabled, but the Ollama URL is invalid.",
            });
            return;
        }

        int reviewedRecords = 0;
        int maxRecordsPerFile = Math.Max(settings.MaxRecordsPerFile, 0);
        DateTime startedAt = DateTime.UtcNow;
        int maxSecondsPerFile = Math.Clamp(settings.MaxSecondsPerFile, 5, 600);

        foreach (BibcatRecord record in records.Records)
        {
            if (maxRecordsPerFile == 0 || reviewedRecords >= maxRecordsPerFile)
            {
                return;
            }

            if (DateTime.UtcNow - startedAt > TimeSpan.FromSeconds(maxSecondsPerFile))
            {
                warnings.Add(new TextQualityWarningModel
                {
                    Level = TextQualityWarningLevel.Information,
                    Message = $"AI text check stopped after {maxSecondsPerFile} seconds. The export was still created without changes.",
                });
                return;
            }

            string abstractText = WebUtility.HtmlDecode(record.Abstract?.Text ?? string.Empty);

            if (string.IsNullOrWhiteSpace(abstractText))
            {
                continue;
            }

            if (!HasLikelySplitWordArtifact(abstractText))
            {
                continue;
            }

            reviewedRecords++;
            TextQualityWarningModel? warning = await ReviewAbstractAsync(baseUri, settings, record.RecordNo, abstractText);

            if (warning != null)
            {
                warnings.Add(warning);

                if (IsAiUnavailableWarning(warning))
                {
                    return;
                }
            }
        }
    }

    private async Task<TextQualityWarningModel?> ReviewAbstractAsync(Uri baseUri, TextQualityReviewSettings settings, string recordNumber, string abstractText)
    {
        int timeoutSeconds = Math.Clamp(settings.TimeoutSeconds, 3, 120);
        using CancellationTokenSource timeout = new(TimeSpan.FromSeconds(timeoutSeconds));

        try
        {
            object request = new
            {
                model = settings.Model,
                stream = false,
                format = "json",
                options = new
                {
                    temperature = 0,
                },
                messages = new[]
                {
                    new
                    {
                        role = "system",
                        content = """
You review English bibliographic abstracts for text extraction defects.
Detect only likely technical extraction defects caused by copying text from web pages through Word.
Common defects are:
- a single word incorrectly split by a space, for example "hu man", "im pact", "re quires", "percep tual", "study ing", or "repro ducibility"
- unknown/replacement markers such as "<?>" or "�" inside running text
Do not rewrite style, terminology, names, titles, abbreviations, or scientific meaning.
Do not flag correct hyphenated compounds such as "ground-borne", "ship-bridge", "long-term", or "cost-effective".
Do not flag acronyms, parenthesized acronyms, possessives, punctuation, spelling variants, or ordinary grammar.
If the only possible change is removing or adding hyphens, return hasIssues false.
Set hasIssues true only when suggestedText fixes one or more obvious split-word or unknown-marker artifacts and preserves the original meaning.
Return strict JSON with these properties:
hasIssues: boolean
message: short string
issues: array of short strings
suggestedText: the minimally corrected abstract if hasIssues is true, otherwise empty string
""",
                    },
                    new
                    {
                        role = "user",
                        content = $"Record: {recordNumber}\n\nAbstract:\n{abstractText}",
                    },
                },
            };

            string json = JsonSerializer.Serialize(request, _jsonOptions);
            using StringContent content = new(json, Encoding.UTF8, "application/json");
            Uri requestUri = new(baseUri, "/api/chat");

            using HttpResponseMessage response = await _httpClient.PostAsync(requestUri, content, timeout.Token);

            if (!response.IsSuccessStatusCode)
            {
                return new TextQualityWarningModel
                {
                    RecordNumber = recordNumber,
                    Level = TextQualityWarningLevel.Information,
                    Message = $"AI text check could not review record {recordNumber}. Ollama returned {(int)response.StatusCode}.",
                };
            }

            string responseJson = await response.Content.ReadAsStringAsync(timeout.Token);
            OllamaChatResponse? chatResponse = JsonSerializer.Deserialize<OllamaChatResponse>(responseJson, _jsonOptions);
            LlmTextQualityResponse? review = TryDeserializeReview(chatResponse?.Message?.Content);

            if (review?.HasIssues != true)
            {
                return null;
            }

            string suggestedText = review.SuggestedText ?? string.Empty;

            if (!HasMeaningfulSuggestedCorrection(abstractText, suggestedText))
            {
                return null;
            }

            return new TextQualityWarningModel
            {
                RecordNumber = recordNumber,
                Level = settings.WarningLevel,
                Message = string.IsNullOrWhiteSpace(review.Message)
                    ? $"Record {recordNumber} may contain text extraction defects."
                    : review.Message,
                Issues = review.Issues ?? [],
                SuggestedText = suggestedText,
            };
        }
        catch (Exception exception) when (exception is HttpRequestException or TaskCanceledException or JsonException)
        {
            return new TextQualityWarningModel
            {
                RecordNumber = recordNumber,
                Level = TextQualityWarningLevel.Information,
                Message = $"AI text check could not review record {recordNumber}. Ollama may be unavailable, busy, or missing model '{settings.Model}'.",
            };
        }
    }

    private static bool HasLikelySplitWordArtifact(string text)
    {
        if (UnknownCharacterMarkerPattern.IsMatch(text))
        {
            return true;
        }

        foreach (Match match in SuspiciousSplitWordPattern.Matches(text))
        {
            string[] words = match.Value.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            if (words.Length != 2)
            {
                continue;
            }

            string first = words[0].ToLowerInvariant();
            string second = words[1].ToLowerInvariant();

            if (AllowedShortWords.Contains(first) || AllowedShortWords.Contains(second))
            {
                continue;
            }

            if (first.Length <= 3 || second.Length <= 3)
            {
                return true;
            }
        }

        return false;
    }

    private static bool HasMeaningfulSuggestedCorrection(string originalText, string suggestedText)
    {
        if (string.IsNullOrWhiteSpace(suggestedText))
        {
            return false;
        }

        string normalizedOriginal = NormalizeForComparison(originalText);
        string normalizedSuggested = NormalizeForComparison(suggestedText);

        if (normalizedOriginal == normalizedSuggested)
        {
            return false;
        }

        if (UnknownCharacterMarkerPattern.IsMatch(originalText) && !UnknownCharacterMarkerPattern.IsMatch(suggestedText))
        {
            return true;
        }

        return HasLikelySplitWordArtifact(originalText)
            && normalizedSuggested.Length >= normalizedOriginal.Length - 20;
    }

    private static string NormalizeForComparison(string text)
    {
        return Regex.Replace(text, @"\s+", " ")
            .Replace(" - ", "-", StringComparison.Ordinal)
            .Trim();
    }

    private LlmTextQualityResponse? TryDeserializeReview(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return null;
        }

        return JsonSerializer.Deserialize<LlmTextQualityResponse>(content, _jsonOptions);
    }

    private static bool IsAiUnavailableWarning(TextQualityWarningModel warning)
    {
        return warning.Level == TextQualityWarningLevel.Information
            && warning.Message.Contains("AI text check could not review", StringComparison.OrdinalIgnoreCase);
    }

    private sealed class OllamaChatResponse
    {
        public OllamaMessage? Message { get; set; }
    }

    private sealed class OllamaMessage
    {
        public string Content { get; set; } = string.Empty;
    }

    private sealed class LlmTextQualityResponse
    {
        public bool HasIssues { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string>? Issues { get; set; }
        public string? SuggestedText { get; set; }
    }
}
