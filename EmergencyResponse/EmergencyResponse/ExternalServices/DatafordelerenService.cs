using EmergencyResponse.DTO;
using EmergencyResponse.ExternalServices.Interfaces;
using EmergencyResponse.Model;
using EmergencyResponse.Services;

namespace EmergencyResponse.ExternalServices
{
    public class DatafordelerenService : IDatafordelerenService
    {
        private readonly HttpClient _httpClient;
        private readonly IApiMessageHandler _apiMessageHandler;
        public DatafordelerenService(HttpClient httpClient, IApiMessageHandler apiMessageHandler)
        {
            _httpClient = httpClient;
            _apiMessageHandler = apiMessageHandler;
        }
        public Task<List<Address>> GetAddressesInBuilding(Address address)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetJordstykkeFromDAR(string addressId)
        {
            string URL = $"DAR/DAR/2.0.0/rest/adresse?" +
                $"id={addressId}" +
                "&username=QRUSLIHSDE&password=SOFTWAREKval!tet2024" +
                "&format=json";
            var response = await _httpClient.GetAsync(URL);
            var content = await response.Content.ReadAsStringAsync();
            return _apiMessageHandler.GetNestedPropertyFromJson(content, "husnummer", "jordstykke");
        }
    }
}
