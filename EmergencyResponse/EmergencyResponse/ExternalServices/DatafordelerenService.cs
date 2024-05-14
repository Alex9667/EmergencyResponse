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
        public async Task<List<AddressDTO>> GetAddressesInBuilding(AddressDTO address)
        {
            var jordstykkeNummer = await GetJordstykkeFromDAR(address.AddressId);
            var husnummerId = await GetHusnummerIdFromDAR(address.AddressId);
            var grundId = await GetGrundIdFromBBR(jordstykkeNummer);
            var buildingId = await GetBuildingIdFromBBR(grundId, husnummerId);
            return await GetUnitsIdsFromBBR(buildingId);
        }

        public async Task<string> CallFromDARApi(string addressId)
        {
            string URL = $"DAR/DAR/2.0.0/rest/adresse?" +
                $"id={addressId}" +
                "&username=QRUSLIHSDE&password=SOFTWAREKval!tet2024" +
                "&format=json";
            var response = await _httpClient.GetAsync(URL);
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetJordstykkeFromDAR(string addressId)
        {
            string json = await CallFromDARApi(addressId);
            return _apiMessageHandler.GetNestedPropertyFromJson(json, "husnummer", "jordstykke");
        }

        public async Task<string> GetHusnummerIdFromDAR(string addressId)
        {
            string json = await CallFromDARApi(addressId);
            return _apiMessageHandler.GetNestedPropertyFromJson(json, "husnummer", "id_lokalId");
        }

        public async Task<string> GetGrundIdFromBBR(string jordstykke)
        {
            string URL = $"BBR/BBRPublic/1/rest/grund?" +
                $"jordstykke={jordstykke}" +
                $"&username=QRUSLIHSDE&password=SOFTWAREKval!tet2024&format=json";
            var response = await _httpClient.GetAsync(URL);
            var content = await response.Content.ReadAsStringAsync();
            return _apiMessageHandler.GetPropertyFromJson(content, "id_lokalId");
        }
        public async Task<string> GetBuildingIdFromBBR(string grundId, string husnummerId)
        {
            string URL = $"BBR/BBRPublic/1/rest/bygning?" +
                $"grund={grundId}" +
                $"&username=QRUSLIHSDE&password=SOFTWAREKval!tet2024&format=json";
            var response = await _httpClient.GetAsync(URL);
            var content = await response.Content.ReadAsStringAsync();
            var buildingElement = _apiMessageHandler.GetBuildingIdFromJsonElement(content, husnummerId, "opgangList", "opgang", "adgangFraHusnummer");
            var buildingId = _apiMessageHandler.GetPropertyFromJson(buildingElement, "id_lokalId");
            return buildingId;
        }

        public async Task<List<AddressDTO>> GetUnitsIdsFromBBR(string buildingId)
        {
            string URL = $"BBR/BBRPublic/1/rest/bygning?" +
                $"bygning={buildingId}" +
                $"&username=QRUSLIHSDE&password=SOFTWAREKval!tet2024&format=json";
            var response = await _httpClient.GetAsync(URL);
            var content = await response.Content.ReadAsStringAsync();
            List<string> unitIds = _apiMessageHandler.GetPropertiesFromJson(content, "id_lokalId");
            var units = new List<AddressDTO>();
            foreach (var id in unitIds)
            {
                var json = await CallFromDARApi(id);
                var unit = new AddressDTO(_apiMessageHandler.ParseAddressFromDAR(json));
                units.Add(unit);
            }
            return units;
        }
    }
}
