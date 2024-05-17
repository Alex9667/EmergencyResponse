using EmergencyResponse.ExternalServices.Mapping;
using EmergencyResponse.Model;
using EmergencyResponse.Services.DataExport.Mapping;
using EmergencyResponse.SharedClasses.Validation.Interfaces;
using Newtonsoft.Json;

namespace EmergencyResponse.Services.DataExport
{
    public class JsonDataExportService : IDataExportService
    {
        private readonly JsonSerializer _serializer;
        private readonly ILanguageStrategyFactory _languageStrategyFactory;

        public JsonDataExportService(JsonSerializer serializer, ILanguageStrategyFactory languageStrategyFactory, IAddressValidator addressValidator)
        {
            _serializer = serializer;
            _languageStrategyFactory = languageStrategyFactory;
        }

        public async Task<string> ExportAddressesToJson(List<Address> addresses, OutputLanguage language)
        {
            StringWriter sw = new StringWriter();
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.WriteStartArray();
                //Which strategy for language mapping the Jsonconverter should use, based on the OutputLanguage enum
                var languageStrategy = _languageStrategyFactory.Create(language);
                var converter = new AddressJsonConverter(languageStrategy);
                //Adds the AddressJsonConverter to our jsonSerializer
                _serializer.Converters.Add(converter);

                foreach (var address in addresses)
                {
                    //Serializes 
                    _serializer.Serialize(writer, address);
                }

                _serializer.Converters.Remove(converter);
                writer.WriteEndArray();
            }
            return sw.ToString();
        }
    }
}
