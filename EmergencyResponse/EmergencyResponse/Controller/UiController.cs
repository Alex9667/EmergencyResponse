using EmergencyResponse.Model;
using EmergencyResponse.Services.DataExport;
using Microsoft.AspNetCore.Components;
using System.Text;
using EmergencyResponse.ExternalServices;
using EmergencyResponse.ExternalServices.Interfaces;
inject IDataforsyningService DataforsyningService;
inject IDataExportService DataExportService:
inject IJSRuntime JS;

namespace EmergencyResponse.Controller
{
    public class UiController
    {
        private Address? address;
        private List<Address>? addresses;
        private int radius = 10; // Default radius
        private OutputLanguage selectedLanguage = OutputLanguage.English;



        private async Task FetchAddress()
        {
            // Example address query, replace with dynamic data if needed
            Address queryAddress = new Address("Rødegårdsvej", "98C", "3", "th", "5000", "Odense C");
            address = await DataforsyningService.GetAddressAsync(queryAddress);
        }

        private async Task FetchAddresses()
        {
            addresses = await DataforsyningService.GetAddressesInCircleAsync(address, radius);
        }

        private void HandleRadiusChange(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out var newRadius))
            {
                radius = newRadius;
            }
        }

        private void HandleLanguageChange(ChangeEventArgs e)
        {
            if (Enum.TryParse<OutputLanguage>(e.Value?.ToString(), out var language))
            {
                selectedLanguage = language;
            }
        }

        private async Task ExportCSV()
        {
            var csvContent = await DataExportService.ExportAddressesToJson(addresses, selectedLanguage);
            byte[] bytes = Encoding.UTF8.GetBytes(csvContent);
            await JS.InvokeVoidAsync("saveAsFile", "Addresses.csv", Convert.ToBase64String(bytes));
        }
    }
}
