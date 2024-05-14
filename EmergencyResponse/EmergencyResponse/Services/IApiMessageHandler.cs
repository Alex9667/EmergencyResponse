using EmergencyResponse.DTO;
using EmergencyResponse.Model;
using System.Text.Json;

namespace EmergencyResponse.Services
{
    public interface IApiMessageHandler
    {
        public List<AddressDTO> ParseAddresses(string json);
        public string GetPropertyFromJson(string json, string propertyName);
        string GetNestedPropertyFromJson(string json, params string[] keyValues);
        public string FindJsonElement(JsonElement element, params string[] keyValues);
    }
}
