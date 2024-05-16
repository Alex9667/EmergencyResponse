using EmergencyResponse.SharedClasses.Mapping.interfaces;

namespace EmergencyResponse.SharedClasses.Mapping
{
    public class EnglishLanguageStrategy : ILanguageStrategy
    {
        public string GetKey(string keyType)
        {
            var keys = new Dictionary<string, string>
            {
                ["streetName"] = "streetName",
                ["houseNumber"] = "houseNumber",
                ["floor"] = "floor",
                ["door"] = "door",
                ["postalCode"] = "postalCode",
                ["postalCodeName"] = "cityName"
            };
            return keys[keyType];
        }
    }
}
