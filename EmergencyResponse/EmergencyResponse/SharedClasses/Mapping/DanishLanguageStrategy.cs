using EmergencyResponse.SharedClasses.Mapping.interfaces;

namespace EmergencyResponse.SharedClasses.Mapping
{
    public class DanishLanguageStrategy : ILanguageStrategy
    {
        public string GetKey(string keyType)
        {
            var keys = new Dictionary<string, string>
            {
                ["streetName"] = "vejnavn",
                ["houseNumber"] = "husnr",
                ["floor"] = "etage",
                ["door"] = "dør",
                ["postalCode"] = "postnr",
                ["postalCodeName"] = "by"
            };
            return keys[keyType];
        }
    }

}
