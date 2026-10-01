using RoadToTrid.Data.Interfaces;
using RoadToTrid.Services;
using RoadToTrid.Services.Mapping;
using RoadToTrid.Services.Mapping.Bibcat;
using RoadToTrid.Services.Mapping.Pdb;
using RoadToTrid.Components;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddScoped<XmlFileService>();
builder.Services.AddScoped<XmlSerializationService>();
builder.Services.AddScoped<XmlProcessingService>();
builder.Services.AddSingleton<ApplicationSettingsService>();
builder.Services.AddHttpClient<TextQualityReviewService>();
builder.Services.AddScoped<IRecordMappingService, BibcatRecordMappingService>();
builder.Services.AddScoped<IRecordMappingService, PdbRecordMappingService>();
builder.Services.AddScoped<MarcToTridMappingService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();
