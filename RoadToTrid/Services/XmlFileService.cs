using RoadToTrid.Data.Models;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace RoadToTrid.Services;

public class XmlFileService
{
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly long maxFileSize = 1024 * 1024 * 3; // 3 MB

    public XmlFileService(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
    }

    public XmlFileProcessingModel LoadXmlFile(IFormFile file)
    {
        return new XmlFileProcessingModel
        {
            InputFile = file,
            InputFileName = file.FileName,
            InputFileSize = file.Length / 1024,
            OutputFileName = file.FileName.Replace("koha", "TRID-export").Replace("Koha", "TRID-export"),
        };
    }

    public async Task<XDocument?> ParseXmlFileToXdocumentAsync(IFormFile? file, XmlFileProcessingModel processingModel)
    {
        if (file is null)
        {
            processingModel.ErrorMessage = "No file was uploaded.";
            return null;
        }

        if (file.Length > maxFileSize)
        {
            processingModel.ErrorMessage = "The XML file is too large. The maximum size is 3 MB.";
            return null;
        }

        try
        {
            using Stream fileStream = file.OpenReadStream();
            using var reader = new StreamReader(fileStream, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);

            string content = await reader.ReadToEndAsync();

            if (!content.Contains("encoding=\"utf-8\"", StringComparison.OrdinalIgnoreCase))
            {
                processingModel.ErrorMessage = "The XML file must declare UTF-8 encoding explicitly.";
                return null;
            }

            return XDocument.Parse(content);
        }
        catch (Exception ex)
        {
            processingModel.ErrorMessage = $"Failed to parse XML file. {ex.Message}";
            return null;
        }
    }

    public async Task<bool> ValidateXmlDocumentAgainstSchema(XDocument document, XmlFileProcessingModel file)
    {
        const string requiredNamespace = "http://www.loc.gov/MARC21/slim";

        string? actualNamespace = document.Root?.Name.NamespaceName;

        if (string.IsNullOrWhiteSpace(actualNamespace) || actualNamespace != requiredNamespace)
        {
            file.ErrorMessage = "The XML document does not use the expected namespace.";
            return false;
        }

        XmlSchemaSet schemaSet = new();
        string targetNamespace = "http://www.loc.gov/MARC21/slim";
        string schemaPath = Path.Combine(_webHostEnvironment.WebRootPath, "schemas", "MARC21slim.xsd");

        try
        {
            string schemaContent = await File.ReadAllTextAsync(schemaPath);
            using StringReader reader = new(schemaContent);
            schemaSet.Add(targetNamespace, XmlReader.Create(reader));
        }
        catch (Exception ex)
        {
            file.ErrorMessage = $"Error loading XML schema. {ex.Message}";
            return false;
        }

        bool errors = false;

        try
        {
            document.Validate(schemaSet, (_, e) =>
            {
                file.ErrorMessage = e.Message;
                errors = true;
            });
        }
        catch (Exception ex)
        {
            file.ErrorMessage = ex.Message;
            return false;
        }

        return !errors;
    }

    public async Task<bool> ValidateXmlWithMultipleSchemas(XDocument document)
    {
        XmlSchemaSet schemaSet = new();
        string targetNamespace = "http://www.loc.gov/MARC21/slim";
        string mainSchemaPath = Path.Combine(_webHostEnvironment.WebRootPath, "schemas", "TRISrecords_export.xsd");
        string extensionSchemaPath = Path.Combine(_webHostEnvironment.WebRootPath, "schemas", "Transguide_attributes.xsd");

        try
        {
            string mainSchemaContent = await File.ReadAllTextAsync(mainSchemaPath);
            using StringReader mainSchemaReader = new(mainSchemaContent);

            string extensionSchemaContent = await File.ReadAllTextAsync(extensionSchemaPath);
            using StringReader extensionSchemaReader = new(extensionSchemaContent);

            schemaSet.Add(targetNamespace, XmlReader.Create(mainSchemaReader));
            schemaSet.Add(targetNamespace, XmlReader.Create(extensionSchemaReader));
        }
        catch
        {
            return false;
        }

        bool errors = false;

        try
        {
            document.Validate(schemaSet, (_, _) => errors = true);
        }
        catch
        {
            return false;
        }

        return !errors;
    }
}
