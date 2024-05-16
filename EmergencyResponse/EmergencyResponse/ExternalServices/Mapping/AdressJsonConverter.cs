namespace EmergencyResponse.ExternalServices.Mapping
{
    using EmergencyResponse.Model;
    using EmergencyResponse.Services.DataExport;
    using EmergencyResponse.Services.DataExport.Mapping;
    using EmergencyResponse.SharedClasses.Mapping.interfaces;
    using EmergencyResponse.SharedClasses.Mapping;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class AddressJsonConverter : JsonConverter<Address>
    {
        private readonly ILanguageStrategy _languageStrategy;


        public AddressJsonConverter (ILanguageStrategy languageStrategy)
        {
            _languageStrategy = languageStrategy;
        }

        public AddressJsonConverter()
       : this(new EnglishLanguageStrategy())
        {
        }

        public override Address ReadJson(JsonReader reader, Type objectType, Address existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            return new Address(
                streetName: (string)obj["vejnavn"],
                houseNumber: (string)obj["husnr"],
                floor: (string)obj["etage"],
                door: (string)obj["dør"],
                postalCode: (string)obj["postnr"],
                postalCodeName: (string)obj["postnrnavn"]
            );
        }

        public override void WriteJson(JsonWriter writer, Address value, JsonSerializer serializer)
        {
            JObject obj = new JObject
            {
                [_languageStrategy.GetKey("streetName")] = value.StreetName,
                [_languageStrategy.GetKey("houseNumber")] = value.HouseNumber,
                [_languageStrategy.GetKey("floor")] = value.Floor ?? string.Empty,
                [_languageStrategy.GetKey("door")] = value.Door ?? string.Empty,
                [_languageStrategy.GetKey("postalCode")] = value.PostalCode,
                [_languageStrategy.GetKey("postalCodeName")] = value.PostalCodeName
            };
            obj.WriteTo(writer);
        }
    }
}
