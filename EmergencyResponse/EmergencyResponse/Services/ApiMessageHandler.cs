
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

        public Address ParseSingleAddressFromDAR(string json)
        {
            var streetName = GetNestedPropertyFromJson(json, "husnummer", "navngivenVej", "vejnavn");
            var houseNumber = GetNestedPropertyFromJson(json, "husnummer", "husnummertekst");
            var floor = GetPropertyFromJson(json, "etagebetegnelse")[0];
            var door = GetPropertyFromJson(json, "dørbetegnelse")[0];
            var postalCode = GetNestedPropertyFromJson(json, "husnummer", "postnummer", "postnr");
            var postalCodeName = GetNestedPropertyFromJson(json, "husnummer", "postnummer", "navn");
            var result = new Address(streetName, houseNumber, floor, door, postalCode, postalCodeName);
            return result;
        }

        public List<string> GetPropertyFromJson(string json, string propertyName)
        {
            var obj = JsonSerializer.Deserialize<JsonElement>(json);
            List<string> result = new List<string>();
            if (obj.ValueKind == JsonValueKind.Array)
            {
                for (int i = 0; i < obj.GetArrayLength(); i++)
                {
                    result.Add(obj[i].GetProperty(propertyName).GetString());
                }
            }
            else
            {
                result.Add(obj.GetProperty(propertyName).GetString());
            }
            return result;
        }

        public string GetNestedPropertyFromJson(string json, params string[] propertyNames)
        {
            var jElement = JsonSerializer.Deserialize<List<JsonElement>>(json);
            return FindJsonElement(jElement[0], propertyNames);
        }

        private string FindJsonElement(JsonElement element, params string[] propertyNames)
        {
            if (element.TryGetProperty(propertyNames[0], out var property))
            {
                if (property.ValueKind == JsonValueKind.Object)
                {
                    return FindJsonElement(property, propertyNames.Take(new Range(1, propertyNames.Length)).ToArray());
                }
                else if (property.ValueKind == JsonValueKind.Array)
                {
                    for (int i = 0; i < property.GetArrayLength(); i++)
                    {
                        return FindJsonElement(property[i], propertyNames.Take(new Range(1, propertyNames.Length)).ToArray());
                    }
                }
                else
                {
                    return property.GetString();
                }
            }
            return null;
        }

        public string GetBBRBuildingIdFromJson(string content, string husnummerId, params string[] propertyNames)
        {
            var element = GetBuildingElementFromHusnummerId(content, husnummerId, propertyNames);
            return GetPropertyFromJson(element, "id_lokalId")[0];
        }

        private string GetBuildingElementFromHusnummerId(string content, string husnummerId, params string[] propertyNames)
        {
            var elements = JsonSerializer.Deserialize<List<JsonElement>>(content);
            JsonElement result = new JsonElement();
            foreach (var element in elements)
            {
                if (element.GetRawText().Contains(husnummerId))
                {
                    result = element;
                }
            }
            if (result.ValueKind == JsonValueKind.Null)
            {
                throw new Exception($"No JsonElements conatins husnummerId: {husnummerId}");
            }
            return result.GetRawText();
        }

    }
}
