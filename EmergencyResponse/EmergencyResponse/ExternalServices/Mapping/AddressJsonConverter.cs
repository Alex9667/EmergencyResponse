namespace EmergencyResponse.ExternalServices.Mapping
{
    using EmergencyResponse.Model;
    using EmergencyResponse.Services.DataExport;
    using EmergencyResponse.Services.DataExport.Mapping;
    using EmergencyResponse.SharedClasses.Mapping.interfaces;
    using EmergencyResponse.SharedClasses.Mapping;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using EmergencyResponse.SharedClasses.Validation.Interfaces;
    using EmergencyResponse.SharedClasses.Validation;

    public class AddressJsonConverter : JsonConverter<Address>
    {
        private readonly ILanguageStrategy _languageStrategy;
        private readonly IAddressValidator _validator;


        public AddressJsonConverter (ILanguageStrategy languageStrategy, IAddressValidator validator)
        {
            _languageStrategy = languageStrategy;
            _validator = validator;
        }

        public AddressJsonConverter()
       : this(new EnglishLanguageStrategy(), new AddressValidator())
        {
        }

        public override Address ReadJson(JsonReader reader, Type objectType, Address existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            var address = new Address(
                streetName: (string)obj["vejnavn"],
                houseNumber: (string)obj["husnr"],
                floor: (string)obj["etage"],
                door: (string)obj["dør"],
                postalCode: (string)obj["postnr"],
                postalCodeName: (string)obj["postnrnavn"]
            );

            // Validate the address using the injected validator
            _validator.Validate(address);

            return address;
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
