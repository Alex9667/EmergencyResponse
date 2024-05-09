namespace EmergencyResponse.ExternalServices
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using EmergencyResponse.Model;
    using EmergencyResponse.ExternalServices.Interfaces;

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
    }

}
