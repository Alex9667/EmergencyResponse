namespace EmergencyResponse.ExternalServices.Mapping
{
    using EmergencyResponse.Model;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class AddressJsonConverter : JsonConverter<Address>
    {
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
            throw new NotImplementedException("Unnecessary because CanWrite is false. The type will skip the converter.");
        }

        public override bool CanWrite => false;
    }
}
