using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Moq;
using EmergencyResponse.ExternalServices.Mapping;
using EmergencyResponse.SharedClasses.Mapping;
using EmergencyResponse.Model;
using EmergencyResponse.SharedClasses.Mapping.interfaces;

namespace EmergencyResponseTest
{
    public class AddressJsonConverterTests
    {
        private readonly AddressJsonConverter _converter;
        private readonly Mock<ILanguageStrategy> _languageStrategyMock;
        private readonly Mock<JsonReader> _readerMock;
        private readonly JsonSerializer _serializer;

        public AddressJsonConverterTests()
        {
            _languageStrategyMock = new Mock<ILanguageStrategy>();
            _converter = new AddressJsonConverter(_languageStrategyMock.Object);
            _readerMock = new Mock<JsonReader>();
            _serializer = new JsonSerializer();
        }

        // Setup language strategy mock for English
        private void SetupEnglishLanguageStrategy()
        {
            _languageStrategyMock.Setup(x => x.GetKey("streetName")).Returns("streetName");
            _languageStrategyMock.Setup(x => x.GetKey("houseNumber")).Returns("houseNumber");
            _languageStrategyMock.Setup(x => x.GetKey("floor")).Returns("floor");
            _languageStrategyMock.Setup(x => x.GetKey("door")).Returns("door");
            _languageStrategyMock.Setup(x => x.GetKey("postalCode")).Returns("postalCode");
            _languageStrategyMock.Setup(x => x.GetKey("postalCodeName")).Returns("cityName");
        }

        // Setup language strategy mock for Danish
        private void SetupDanishLanguageStrategy()
        {
            _languageStrategyMock.Setup(x => x.GetKey("streetName")).Returns("vejnavn");
            _languageStrategyMock.Setup(x => x.GetKey("houseNumber")).Returns("husnr");
            _languageStrategyMock.Setup(x => x.GetKey("floor")).Returns("etage");
            _languageStrategyMock.Setup(x => x.GetKey("door")).Returns("dør");
            _languageStrategyMock.Setup(x => x.GetKey("postalCode")).Returns("postnr");
            _languageStrategyMock.Setup(x => x.GetKey("postalCodeName")).Returns("by");
        }

        //Tests when reading json that it maps the specific values to the right properties using the Json keys
        [Fact]
        public void ReadJson_ValidJsonDanish_CreatesCorrectAddress()
        {
            var jsonData = @"{
            'vejnavn': 'Elm Street',
            'husnr': '123',
            'etage': '2',
            'dør': '1A',
            'postnr': '1234',
            'postnrnavn': 'Springfield'
                }";

            //StringReader to simulate input from a file or stream
            using (var stringReader = new StringReader(jsonData))
            using (var jsonReader = new JsonTextReader(stringReader)) // Use JsonTextReader, which implements JsonReader
            {
                var address = _converter.ReadJson(jsonReader, typeof(Address), null, false, _serializer);
                Assert.Equal("Elm Street", address.StreetName);
                Assert.Equal("123", address.HouseNumber);
                Assert.Equal("2", address.Floor);
                Assert.Equal("1A", address.Door);
                Assert.Equal("1234", address.PostalCode);
                Assert.Equal("Springfield", address.PostalCodeName);
            }
        }

        [Fact]
        public void WriteJson_WithEnglishLanguageStrategy_WritesCorrectJson()
        {
            SetupEnglishLanguageStrategy();
            var address = new Address("Elm Street", "123", "2", "1A", "1234", "Springfield");

            var stringWriter = new StringWriter();
            var jsonWriter = new JsonTextWriter(stringWriter);

            _converter.WriteJson(jsonWriter, address, _serializer);
            jsonWriter.Flush(); // Ensure all JSON data is written to the StringWriter

            string output = stringWriter.ToString();
            Assert.Contains("\"streetName\":\"Elm Street\"", output);
            Assert.Contains("\"houseNumber\":\"123\"", output);
            Assert.Contains("\"floor\":\"2\"", output);
            Assert.Contains("\"door\":\"1A\"", output);
            Assert.Contains("\"postalCode\":\"1234\"", output);
            Assert.Contains("\"cityName\":\"Springfield\"", output);
        }

        [Fact]
        public void WriteJson_WithDanishLanguageStrategy_WritesCorrectJson()
        {
            SetupDanishLanguageStrategy();
            var address = new Address("Elm Street", "123", "2", "1A", "1234", "Springfield");

            var stringWriter = new StringWriter();
            var jsonWriter = new JsonTextWriter(stringWriter);

            _converter.WriteJson(jsonWriter, address, _serializer);
            jsonWriter.Flush(); // Ensure all JSON data is written to the StringWriter

            string output = stringWriter.ToString();
            Assert.Contains("\"vejnavn\":\"Elm Street\"", output);
            Assert.Contains("\"husnr\":\"123\"", output);
            Assert.Contains("\"etage\":\"2\"", output);
            Assert.Contains("\"dør\":\"1A\"", output);
            Assert.Contains("\"postnr\":\"1234\"", output);
            Assert.Contains("\"by\":\"Springfield\"", output);
        }
    }
}
