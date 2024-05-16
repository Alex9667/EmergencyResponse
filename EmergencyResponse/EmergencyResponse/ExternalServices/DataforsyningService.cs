namespace EmergencyResponse.ExternalServices
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using EmergencyResponse.Model;
    using EmergencyResponse.ExternalServices.Interfaces;
    using EmergencyResponse.ExternalServices.Mapping;
    using EmergencyResponse.DTO;
    using System.Text.Json;
    using EmergencyResponse.Services;

    public class DataforsyningenService : IDataforsyningService
    {
        private readonly HttpClient _httpClient;
        private readonly IApiMessageHandler _apiMessageHandler;

        public DataforsyningenService(HttpClient httpClient, IApiMessageHandler apiMessageHandler)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiMessageHandler = apiMessageHandler;
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


        public async Task<List<Address>> GetAddressesInCircleAsync(Address center, int radius)
        {
            if (center == null || !center.Latitude.HasValue || !center.Longitude.HasValue)
                throw new ArgumentException("Center address must have valid latitude and longitude.");

            string longitudeString = center.Longitude.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string latitudeString = center.Latitude.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
            string radiusString = radius.ToString();
            string url = $"https://api.dataforsyningen.dk/adresser?cirkel={longitudeString},{latitudeString},{radiusString}&struktur=mini";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var addresses = JsonConvert.DeserializeObject<List<Address>>(jsonResponse, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    Converters = new List<JsonConverter> { new AddressJsonConverter() }
                });
                return addresses;
            }
            else
            {
                throw new HttpRequestException($"Failed to fetch address data. Status Code: {response.StatusCode}");
            }
        }



        public async Task<List<AddressDTO>> SearchAddress(AddressDTO address)
        {
            var floorValue = address.Floor == null ? "" : address.Floor.ToString();
            var doorValue = address.Door == null ? "" : address.Door;
            var URL = $"adresser?q={address.StreetName} " +
                $"{address.HouseNumber} " +
                $"{address.PostalCode} " +
                $"{floorValue} " +
                $"{doorValue}&struktur=mini";
            var response = await _httpClient.GetAsync(URL);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException e)
            {
                throw new Exception($"There was an error trying to call the API: {e.Message}");
            }
            var content = await response.Content.ReadAsStringAsync();
            List<AddressDTO> result = _apiMessageHandler.ParseAddressesFromDataForsyning(content);
            return result;
        }
    }

}
