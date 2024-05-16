using EmergencyResponse.ExternalServices;
using EmergencyResponse.ExternalServices.Interfaces;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using EmergencyResponse.Model;
using EmergencyResponse.Services;
using EmergencyResponse.Services.DataExport.Mapping;
using EmergencyResponse.Services.DataExport;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();
builder.Services.AddLogging(config =>
{
    config.AddConsole();  // Or any other logging provider
    config.AddDebug();
});

// Register DataforsyningenService with HttpClient
builder.Services.AddHttpClient<IDataforsyningService, DataforsyningenService>(client =>
{
    // Optionally configure the client here if needed
    client.BaseAddress = new Uri("https://api.dataforsyningen.dk/");
});
builder.Services.AddHttpClient<IDatafordelerenService, DatafordelerenService>(client =>
{
    // Optionally configure the client here if needed
    client.BaseAddress = new Uri("https://services.datafordeler.dk/");
});

builder.Services.AddScoped<JsonSerializer>();
builder.Services.AddScoped<IApiMessageHandler, ApiMessageHandler>();
builder.Services.AddSingleton<ILanguageStrategyFactory, LanguageStrategyFactory>(); 
builder.Services.AddTransient<IDataExportService, JsonDataExportService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
