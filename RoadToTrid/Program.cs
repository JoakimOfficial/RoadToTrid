using RoadToTrid.Data.Interfaces;
using RoadToTrid.Services;
using RoadToTrid.Services.Mapping;
using RoadToTrid.Services.Mapping.Bibcat;
using RoadToTrid.Services.Mapping.Pdb;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<XmlFileService>();
builder.Services.AddScoped<XmlSerializationService>();
builder.Services.AddScoped<XmlProcessingService>();
builder.Services.AddScoped<IRecordMappingService, BibcatRecordMappingService>();
builder.Services.AddScoped<IRecordMappingService, PdbRecordMappingService>();
builder.Services.AddScoped<MarcToTridMappingService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
