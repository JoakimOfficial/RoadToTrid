using RoadToTrid.Data.Models;
using RoadToTrid.Data.Models.TridXmlModels.Bibcat;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RoadToTrid.Services;

public class ApplicationSettingsService
{
    private readonly string _settingsFilePath;
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web)
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
    };

    public ApplicationSettingsService(IWebHostEnvironment environment)
    {
        _settingsFilePath = Path.Combine(environment.ContentRootPath, "settings.json");
    }

    public AvailabilityAgency GetAvailabilityAgency()
    {
        return GetSettings().AvailabilityAgency;
    }

    public TextQualityReviewSettings GetTextQualityReviewSettings()
    {
        return GetSettings().TextQualityReview;
    }

    public ApplicationSettings GetSettings()
    {
        if (!File.Exists(_settingsFilePath))
        {
            ApplicationSettings defaultSettings = new();
            TrySaveDefaultSettings(defaultSettings);
            return defaultSettings;
        }

        try
        {
            string json = File.ReadAllText(_settingsFilePath);
            ApplicationSettings? settings = JsonSerializer.Deserialize<ApplicationSettings>(json, _jsonOptions);

            return settings ?? new ApplicationSettings();
        }
        catch (JsonException)
        {
            return new ApplicationSettings();
        }
        catch (IOException)
        {
            return new ApplicationSettings();
        }
    }

    public void SaveAvailabilityAgency(AvailabilityAgency availabilityAgency)
    {
        ApplicationSettings settings = GetSettings();
        settings.AvailabilityAgency = NormalizeAvailabilityAgency(availabilityAgency);

        SaveSettings(settings);
    }

    public void SaveSettings(ApplicationSettings settings)
    {
        settings.AvailabilityAgency = NormalizeAvailabilityAgency(settings.AvailabilityAgency);
        settings.TextQualityReview = NormalizeTextQualityReviewSettings(settings.TextQualityReview);

        string? settingsDirectory = Path.GetDirectoryName(_settingsFilePath);

        if (!string.IsNullOrWhiteSpace(settingsDirectory))
        {
            Directory.CreateDirectory(settingsDirectory);
        }

        string json = JsonSerializer.Serialize(settings, _jsonOptions);
        File.WriteAllText(_settingsFilePath, json);
    }

    private void TrySaveDefaultSettings(ApplicationSettings settings)
    {
        try
        {
            SaveSettings(settings);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }

    private static AvailabilityAgency NormalizeAvailabilityAgency(AvailabilityAgency? availabilityAgency)
    {
        AvailabilityAgency normalized = availabilityAgency ?? new AvailabilityAgency();

        normalized.Street1 ??= string.Empty;
        normalized.Street2 ??= string.Empty;
        normalized.City ??= string.Empty;
        normalized.Region ??= string.Empty;
        normalized.CountryName ??= string.Empty;
        normalized.PostalCode ??= string.Empty;
        normalized.SiteUrl ??= string.Empty;
        normalized.PartNumber ??= string.Empty;
        normalized.OrderUrl ??= string.Empty;
        normalized.Position ??= string.Empty;
        normalized.Text ??= string.Empty;

        return normalized;
    }

    private static TextQualityReviewSettings NormalizeTextQualityReviewSettings(TextQualityReviewSettings? settings)
    {
        TextQualityReviewSettings normalized = settings ?? new TextQualityReviewSettings();

        normalized.OllamaUrl = string.IsNullOrWhiteSpace(normalized.OllamaUrl)
            ? "http://localhost:11434"
            : normalized.OllamaUrl;
        normalized.Model = string.IsNullOrWhiteSpace(normalized.Model)
            ? "llama3.1"
            : normalized.Model;

        return normalized;
    }
}
