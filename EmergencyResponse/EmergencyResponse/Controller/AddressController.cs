using Microsoft.AspNetCore.Mvc;
using EmergencyResponse.Model;
using EmergencyResponse.Interfaces;
using System.Text.Json;

namespace EmergencyResponse.Controller
{
    public class AddressController : ControllerBase
    {
        HttpClient _httpClient;
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

        public object CreateAddress(string id, string streetName, string houseNumber, int? floor, string? door, string postalCode, string postalCodeName, string addressId)
        {
            return new Address(id, streetName, houseNumber, floor, door, postalCode, postalCodeName, addressId);
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

        public async Task<List<Address>> SearchAddress(Address address)
        {
            var floor = address.Floor == null ? "" : address.Floor.ToString();
            var door = address.Door == null ? "" : address.Door;
            var URL = $"https://api.dataforsyningen.dk/adresser" +
                $"?vejnavn{address.StreetName}" +
                $"&husnr{address.HouseNumber}" +
                $"&postnr{address.PostalCode}" +
                $"&etage={floor}" +
                $"&dør={door}&struktur=mini";
            var response = await _httpClient.GetAsync(URL);
            var result = await JsonSerializer.DeserializeAsync<List<Address>>(response.Content.ReadAsStream());
            return result;
        }
    }
}
