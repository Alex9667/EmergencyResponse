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

namespace EmergencyResponse.Controller
{
    public class AddressController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public AddressController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public Address CreateAddress(string json)
        {
            if (json == null || json == "") return null;
            var result = JsonSerializer.Deserialize<Address>(json);
            if (result != null)
            {
                return result;
            }
            return null;
        }

        public object CreateAddress(string streetName, string houseNumber, int? floor, string? door, string postalCode, string postalCodeName, string addressId)
        {
            return new Address(streetName, houseNumber, floor, door, postalCode, postalCodeName, addressId);
        }


        public Address GetAddress()
        {
            throw new NotImplementedException();
        }

        public object GetAddressesInBuilding(Address address)
        {
            throw new NotImplementedException();
        }

        public object GetAddressID(Address address)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Address>> SearchAddress(AddressDTO address)
        {
            var floorValue = address.Floor == null ? "" : address.Floor.ToString();
            var doorValue = address.Door == null ? "" : address.Door;
            var URL = $"https://api.dataforsyningen.dk/adresser?q={address.StreetName} " +
                $"{address.HouseNumber} " +
                $"{address.PostalCode} " +
                $"{floorValue} " +
                $"{doorValue}&struktur=mini";
            var response = await _httpClient.GetAsync(URL);
            var obj = JsonSerializer.Deserialize<List<JsonElement>>(response.Content.ReadAsStream());
            List<Address> result = new List<Address>();
            foreach (var item in obj)
            {
                var streetname = item.GetProperty("vejnavn").GetString();
                var houseNumber = item.GetProperty("husnr").GetString();
                var postalCode = item.GetProperty("postnr").GetString();
                var postalCodeName = item.GetProperty("postnrnavn").GetString();
                item.TryGetProperty("etage", out var floorProperty);
                int? floor = floorProperty.ValueKind == JsonValueKind.Null ? null : int.Parse(floorProperty.GetString()!);
                item.TryGetProperty("etage", out var doorProperty);
                string door = doorProperty.ValueKind == JsonValueKind.Null ? null : doorProperty.GetString();
                var addressId = item.GetProperty("id").GetString();
                result.Add(new(streetname, houseNumber, floor, door, postalCode, postalCodeName, addressId));
            }
            return result;
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
