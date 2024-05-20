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
            var unitIds = await GetUnitsIdsFromBBR(buildingId);
            var result = await GetUnitAddressesFromDAR(unitIds);
            if (result.Count == 0)
            {
                result.Add(address);
            }
            return result;
        }

        public async Task<string> CallApi(string url)
        {
            var response = await _httpClient.GetAsync(url);
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"There was an error when calling the API: \"{ex.Message}\"");
            }
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> CallDARAddressApi(string addressId)
        {
            string URL = $"DAR/DAR/2.0.0/rest/adresse?" +
                $"id={addressId}" +
                "&username=QRUSLIHSDE&password=SOFTWAREKval!tet2024" +
                "&format=json";
            return await CallApi(URL);
        }

        public async Task<string> GetJordstykkeFromDAR(string addressId)
        {
            string content = await CallDARAddressApi(addressId);
            return _apiMessageHandler.GetNestedPropertyFromJson(content, "husnummer", "jordstykke");
        }

        public async Task<string> GetHusnummerIdFromDAR(string addressId)
        {
            string content = await CallDARAddressApi(addressId);
            return _apiMessageHandler.GetNestedPropertyFromJson(content, "husnummer", "id_lokalId");
        }

        public async Task<string> GetGrundIdFromBBR(string jordstykke)
        {
            string URL = $"BBR/BBRPublic/1/rest/grund?" +
                $"jordstykke={jordstykke}" +
                $"&username=QRUSLIHSDE&password=SOFTWAREKval!tet2024&format=json";
            var content = await CallApi(URL);
            return _apiMessageHandler.GetPropertyFromJson(content, "id_lokalId")[0];
        }
        public async Task<string> GetBuildingIdFromBBR(string grundId, string husnummerId)
        {
            string URL = $"BBR/BBRPublic/1/rest/bygning?" +
                $"grund={grundId}" +
                $"&username=QRUSLIHSDE&password=SOFTWAREKval!tet2024&format=json";
            var content = await CallApi(URL);
            var buildingId = _apiMessageHandler.GetBBRBuildingIdFromJson(content, husnummerId, "opgangList", "opgang", "adgangFraHusnummer");
            return buildingId;
        }

        public async Task<List<string>> GetUnitsIdsFromBBR(string buildingId)
        {
            string URL = $"BBR/BBRPublic/1/rest/enhed?" +
                $"bygning={buildingId}" +
                $"&username=QRUSLIHSDE&password=SOFTWAREKval!tet2024&format=json";
            var content = await CallApi(URL);
            List<string> unitIds = _apiMessageHandler.GetPropertyFromJson(content, "adresseIdentificerer");

            return unitIds;
        }

        public async Task<List<AddressDTO>> GetUnitAddressesFromDAR(List<string> unitIds)
        {
            var units = new List<AddressDTO>();
            foreach (var id in unitIds)
            {
                var DarContent = await CallDARAddressApi(id);
                var unit = new AddressDTO(_apiMessageHandler.ParseSingleAddressFromDAR(DarContent));
                units.Add(unit);
            }
            return units;
        }

    }
}
