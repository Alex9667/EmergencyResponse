using Microsoft.AspNetCore.Mvc;
using EmergencyResponse.Model;
using EmergencyResponse.Interfaces;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Nodes;
using EmergencyResponse.DTO;
using EmergencyResponse.ExternalServices.Interfaces;

namespace EmergencyResponse.Controller
{
    public class AddressController : ControllerBase
    {
        private readonly IDataforsyningService _dataforsyningService;
        private readonly HttpClient _httpClient;

        public AddressController(HttpClient httpClient, IDataforsyningService dataforsyningService)
        {
            _httpClient = httpClient;
            _dataforsyningService = dataforsyningService;
        }

        public async Task<List<AddressDTO>> SearchAddress(AddressDTO address)
        {
            return await _dataforsyningService.SearchAddress(address);
        }
        public async Task<int> GetAddressBFE(string address)
        {
            string bfeStepOne = await _httpClient.GetStringAsync("https://api.dataforsyningen.dk/adresser?q=" + address + "&struktur=mini");
            string addressId;

            // Parse the JSON string into a JsonDocument
            using (JsonDocument doc = JsonDocument.Parse(bfeStepOne))
            {
                // Gets the root element of the JSON text
                JsonElement root = doc.RootElement;

                // Gets the first element of the root
                JsonElement firstElement = root[0];

                // Get the id property
                addressId = firstElement.GetProperty("id").GetString();
            }

            string bfeStepTwo = await _httpClient.GetStringAsync("https://services.datafordeler.dk/DAR/DAR/2.0.0/rest/adresse?id=" + addressId + "&username=QRUSLIHSDE&password=SOFTWAREKval!tet2024");
            string landPiece;

            using (JsonDocument doc = JsonDocument.Parse(bfeStepTwo))
            {
                // Gets the root element of the JSON text
                JsonElement root = doc.RootElement[0];

                // Gets the land piece
                landPiece = root.GetProperty("husnummer").GetProperty("jordstykke").GetString();
            }

            string bfeStepThree = await _httpClient.GetStringAsync("https://services.datafordeler.dk//BBR/BBRPublic/1/rest/grund?jordstykke=" + landPiece + "&&username=QRUSLIHSDE&password=SOFTWAREKval!tet2024");
            int bfe;

            using (JsonDocument doc = JsonDocument.Parse(bfeStepThree))
            {
                // Gets the root element of the JSON text
                JsonElement root = doc.RootElement[0];

                // Gets the land piece
                bfe = root.GetProperty("bestemtFastEjendom").GetProperty("bfeNummer").GetInt32();
            }
            return bfe;
        }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);

}
