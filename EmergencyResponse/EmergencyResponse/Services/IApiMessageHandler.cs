using EmergencyResponse.DTO;
using EmergencyResponse.Model;
using System.Text.Json;

namespace EmergencyResponse.Services
{
    public interface IApiMessageHandler
    {
        public List<AddressDTO> ParseAddressesFromDataForsyning(string json);
        public string GetPropertyFromJson(string json, string propertyName);
        string GetNestedPropertyFromJson(string json, params string[] keyValues);
        public string FindJsonElement(JsonElement element, params string[] keyValues);
        public string GetBuildingIdFromJsonElement(string content, string husnummerId, params string[] keyValues);
        public string GetBuildingElementFromHusnummerId(string content, string husnummerId, params string[] keyValues);
        List<string> GetPropertiesFromJson(string json, string keyValue);
        public Address ParseAddressFromDAR(string json);
    }
}
