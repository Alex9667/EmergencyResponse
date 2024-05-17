using EmergencyResponse.DTO;
using EmergencyResponse.Model;
using EmergencyResponse.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace EmergencyResponseTest
{
    public class ApiMessageHandlerTest
    {
        ApiMessageHandler _apiMessageHandler;
        public ApiMessageHandlerTest()
        {
            _apiMessageHandler = new ApiMessageHandler();
        }
        [Theory]
        [MemberData(nameof(GetAddressesForParseAddresses))]
        public void ApiMessageHandler_ParseAddresses_ShouldReturnAddresses(string json, List<Address> expectedAddresses)
        {
            var result = _apiMessageHandler.ParseAddressesFromDataForsyning(json);
            result.Should().BeEquivalentTo(expectedAddresses);
        }
        [Theory]
        [InlineData("provertyName1", "propertyValue1")]
        [InlineData("name", "Bob")]
        [InlineData("street", "æblevej")]
        public void ApiMessageHandler_GetPropertyFromJson_ShouldReturnProperty(string propertyName, string expectedPropertyValue)
        {
            var json = new JArray(new JObject(new JProperty(propertyName, expectedPropertyValue))).ToString();
            var result = _apiMessageHandler.GetPropertyFromJson(json, propertyName);
            result.Should().BeEquivalentTo(expectedPropertyValue);
        }

        [Theory]
        [MemberData(nameof(GetAddressesForParseAddresses))]
        public void ApiMessageHandler_GetNestedPropertyFromJson_ShouldReturnProperty(string json, string expectedPropertyValue, params string[] propertyNames)
        {
            var result = _apiMessageHandler.GetNestedPropertyFromJson(json, propertyNames);
            result.Should().BeEquivalentTo(expectedPropertyValue);
        }

        public static IEnumerable<Object[]> GetAddressesForParseAddresses()
        {
            return new List<object[]> {
                new object[] {
                    new JArray(new JObject(new JProperty("tree", new JObject(new JProperty("name", "Bob"))))).ToString(),
                    "Bob",
                    new string[]{"tree", "name"}
                },
                new object[] {
                    new JArray(new JObject(new JProperty("country", new JObject(new JProperty("city", new JObject(new JProperty("street", "Æblevej"))))))).ToString(),
                    "Æblevej",
                    new string[]{"country", "city", "street"}
                },
            };
        }
        public static IEnumerable<Object[]> GetJsonForGetNestedPropertyFromJson()
        {
            return new List<object[]> {
                new object[] {
                    File.ReadAllText("./JsonData/DataForsyningenAddressJson.json"),
                    new List<Address> {
                        new Address("Mellemvej", "7", "1", "th", "5230", "odense M", "hkdhkfjskf"),
                        new Address("Mellemvej", "7", "1", "tv", "5230", "odense M", "hkdhkfjskf"),
                        new Address("Mellemvej", "7", "st", "th", "5230", "odense M", "hkdhkfjskf"),
                        new Address("Mellemvej", "7", "st", "tv", "5230", "odense M", "hkdhkfjskf"),
                    },
                },
            };
        }

    }
}
