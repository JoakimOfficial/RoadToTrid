using Microsoft.AspNetCore.Mvc;
using RoadToTrid.Data.Enums;
using RoadToTrid.Services;
using RoadToTrid.ViewModels;
using System.Diagnostics;

namespace RoadToTrid.Controllers;

public class HomeController : Controller
{
    private const int MaxAllowedFiles = 5;
    private readonly XmlProcessingService _xmlProcessingService;

    public HomeController(XmlProcessingService xmlProcessingService)
    {
        _xmlProcessingService = xmlProcessingService;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View(new HomeUploadViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(IReadOnlyCollection<IFormFile> files)
    {
        if (files.Count == 0)
        {
            return View("Index", new HomeUploadViewModel { ErrorMessage = "Choose one or more XML files to upload." });
        }

        if (files.Count > MaxAllowedFiles)
        {
            return View("Index", new HomeUploadViewModel { ErrorMessage = $"You tried to upload {files.Count} files, but only {MaxAllowedFiles} files are allowed at a time." });
        }

        _xmlProcessingService.LoadMultipleFiles(files);

        if (_xmlProcessingService.ShowInvalidFileModal)
        {
            return View("Index", new HomeUploadViewModel { ErrorMessage = "Only XML files are allowed. Please check the file format and try again." });
        }

        foreach (var file in _xmlProcessingService.LoadedFiles)
        {
            await _xmlProcessingService.ProcessXmlFile(file);

            if (file.Status != FileProcessingStatus.Done && string.IsNullOrWhiteSpace(file.ErrorMessage))
            {
                file.Status = FileProcessingStatus.Failed;
                file.ErrorMessage = "The file could not be processed.";
            }
        }

        return View("Index", new HomeUploadViewModel { Files = _xmlProcessingService.LoadedFiles });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Download(string fileName, string content)
    {
        if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(content))
        {
            return BadRequest();
        }

        byte[] bytes = Convert.FromBase64String(content);

        return File(bytes, "application/xml", fileName);
    }

    [HttpGet]
    public IActionResult Documentation()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Settings()
    {
        return View(new Data.Models.TridXmlModels.Bibcat.AvailabilityAgency());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Settings(Data.Models.TridXmlModels.Bibcat.AvailabilityAgency availabilityAgency)
    {
        ViewBag.Saved = true;
        return View(availabilityAgency);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
