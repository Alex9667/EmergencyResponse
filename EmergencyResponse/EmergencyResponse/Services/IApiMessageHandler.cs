using EmergencyResponse.DTO;
using EmergencyResponse.Model;
using System.Text.Json;

namespace EmergencyResponse.Services
{
    public interface IApiMessageHandler
    {

        public List<AddressDTO> ParseAddressesFromDataForsyning(string json);
        public List<string> GetPropertyFromJson(string json, string propertyName);
        string GetNestedPropertyFromJson(string json, params string[] keyValues);
        public string GetBBRBuildingIdFromJson(string content, string husnummerId, params string[] keyValues);
        public Address ParseSingleAddressFromDAR(string json);
    }
}
