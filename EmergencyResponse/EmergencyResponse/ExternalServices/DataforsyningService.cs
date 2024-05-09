namespace EmergencyResponse.ExternalServices
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using EmergencyResponse.Model;
    using EmergencyResponse.ExternalServices.Interfaces;
    using EmergencyResponse.DTO;
    using System.Text.Json;

    public class DataforsyningenService : IDataforsyningService
    {
        private readonly HttpClient _httpClient;

        public DataforsyningenService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<Address> GetAddressAsync(Address address)
        {
            if (address == null)
                throw new ArgumentNullException(nameof(address), "Address cannot be null.");

            // Constructing the query string from the Address object
            var queryString = $"{address.StreetName} {address.HouseNumber} " +
                              $"{address.Floor} {address.Door} " +
                              $"{address.PostalCodeName} {address.PostalCode}";

            // Encode the query string to ensure it is URL-safe
            string url = $"https://api.dataforsyningen.dk/adresser?q={Uri.EscapeDataString(queryString)}&struktur=mini";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var addresses = JsonConvert.DeserializeObject<List<dynamic>>(jsonResponse);
                if (addresses.Count > 0)
                {
                    var addressData = addresses[0];
                    address.SetLatitude((double)addressData.y);
                    address.SetLongitude((double)addressData.x);
                    return address;
                }
                else
                {
                    throw new Exception("No address found with the specified details.");
                }
            }
            throw new Exception($"Failed to fetch address data. Status Code: {response.StatusCode}");
        }

        public async Task<List<AddressDTO>> SearchAddress(AddressDTO address)
        {
            var floorValue = address.Floor == null ? "" : address.Floor.ToString();
            var doorValue = address.Door == null ? "" : address.Door;
            var URL = $"https://api.dataforsyningen.dk/adresser?q={address.StreetName} " +
                $"{address.HouseNumber} " +
                $"{address.PostalCode} " +
                $"{floorValue} " +
                $"{doorValue}&struktur=mini";
            var response = await _httpClient.GetAsync(URL);
            var obj = System.Text.Json.JsonSerializer.Deserialize<List<JsonElement>>(response.Content.ReadAsStream());
            List<AddressDTO> result = new List<AddressDTO>();
            foreach (var item in obj)
            {
                var streetname = item.GetProperty("vejnavn").GetString();
                var houseNumber = item.GetProperty("husnr").GetString();
                var postalCode = item.GetProperty("postnr").GetString();
                var postalCodeName = item.GetProperty("postnrnavn").GetString();
                item.TryGetProperty("etage", out var floorProperty);
                string? floor = floorProperty.ValueKind == JsonValueKind.Null ? null : floorProperty.GetString();
                item.TryGetProperty("dør", out var doorProperty);
                string door = doorProperty.ValueKind == JsonValueKind.Null ? null : doorProperty.GetString();
                var addressId = item.GetProperty("id").GetString();
                var returnAddress = new Address(streetname, houseNumber, floor, door, postalCode, postalCodeName, addressId);
                result.Add(new AddressDTO(returnAddress));
            }
            return result;
        }
    }

}
