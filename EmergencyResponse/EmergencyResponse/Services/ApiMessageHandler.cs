
using EmergencyResponse.DTO;
using EmergencyResponse.Model;
using System.Text.Json;

namespace EmergencyResponse.Services
{
    public class ApiMessageHandler : IApiMessageHandler
    {
        public List<AddressDTO> ParseAddressesFromDataForsyning(string json)
        {
            var obj = System.Text.Json.JsonSerializer.Deserialize<List<JsonElement>>(json);
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
                string? door = doorProperty.ValueKind == JsonValueKind.Null ? null : doorProperty.GetString();
                var addressId = item.GetProperty("id").GetString();
                var returnAddress = new Address(streetname, houseNumber, floor, door, postalCode, postalCodeName, addressId);
                result.Add(new AddressDTO(returnAddress));
            }
            return result;
        }

        public string GetPropertyFromJson(string json, string propertyName)
        {
            var obj = System.Text.Json.JsonSerializer.Deserialize<List<JsonElement>>(json);
            var result = obj[0].GetProperty(propertyName).GetString();
            return result;
        }

        public string GetNestedPropertyFromJson(string json, params string[] keyValues)
        {
            var jElement = JsonSerializer.Deserialize<List<JsonElement>>(json);
            return FindJsonElement(jElement[0], keyValues);
        }

        public string FindJsonElement(JsonElement element, params string[] keyValues)
        {
            if (element.TryGetProperty(keyValues[0], out var property))
            {
                if (property.ValueKind == JsonValueKind.Object)
                {
                    return FindJsonElement(property, keyValues.Take(new Range(1, keyValues.Length)).ToArray());
                }
                else if (property.ValueKind == JsonValueKind.Array)
                {
                    for (int i = 0; i < property.GetArrayLength(); i++)
                    {
                        return FindJsonElement(property[i], keyValues.Take(new Range(1, keyValues.Length)).ToArray());
                    }
                }
                else
                {
                    return property.GetString();
                }
            }
            return null;
        }

        public string GetBuildingIdFromJsonElement(string content, string husnummerId, params string[] keyValues)
        {
            var element = GetBuildingElementFromHusnummerId(content, husnummerId, keyValues);

            return GetPropertyFromJson(element, "id_lokalId");
        }

        public string GetBuildingElementFromHusnummerId(string content, string husnummerId, params string[] keyValues)
        {
            var elements = JsonSerializer.Deserialize<List<JsonElement>>(content);
            JsonElement result = new JsonElement();
            foreach (var element in elements)
            {
                if (FindJsonElement(element, keyValues) == husnummerId)
                {
                    result = element;
                }
            }
            if (result.ValueKind == JsonValueKind.Null)
            {
                throw new Exception($"No JsonElements conatins husnummerId: {husnummerId}");
            }
            var text = result.GetRawText();
            return result.GetRawText();
        }

        public List<string> GetPropertiesFromJson(string json, string keyValue)
        {
            var elements = JsonSerializer.Deserialize<List<JsonElement>>(json);
            List<string> result = new List<string>();
            foreach (var element in elements)
            {
                var id = GetPropertyFromJson(element.GetRawText(), "adresseIdentificerer");
                result.Add(id);
            }
            return result;
        }

        public Address ParseAddressFromDAR(string json)
        {
            var streetName = GetNestedPropertyFromJson(json, "husnummer", "navngivenVej", "vejnavn");
            var houseNumber = GetNestedPropertyFromJson(json, "husnummer", "husnummertekst");
            var floor = GetPropertyFromJson(json, "etagebetegnelse");
            var door = GetPropertyFromJson(json, "dørbetegnelse");
            var postalCode = GetNestedPropertyFromJson(json, "husnummer", "postnummer", "postnr");
            var postalCodeName = GetNestedPropertyFromJson(json, "husnummer", "postnummer", "navn");
            var result = new Address(streetName, houseNumber, floor, door, postalCode, postalCodeName);
            return result;
        }
    }
}
