using EmergencyResponse.DTO;
using EmergencyResponse.ExternalServices;
using EmergencyResponse.ExternalServices.Interfaces;
using EmergencyResponse.Model;
using EmergencyResponse.Services;
using Moq;
using Moq.Protected;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyResponseTest
{
    public class AddressesInBuildingTest
    {
        IDatafordelerenService _datafordelerenService;

        [Theory]
        [MemberData(nameof(GetJordstykkeFromDarTestData))]
        public async Task DataFordelerenService_GetJordstykkeFromDAR_ShouldReturnExpectedNumber(string addressId, string expectedJordstykkeValue)
        {
            var mockHttpClient = new Mock<HttpClient>();
            var mockApiMessageHandler = new Mock<IApiMessageHandler>();
            mockApiMessageHandler.Setup(m => m.GetPropertyFromJson(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedJordstykkeValue);
            _datafordelerenService = new DatafordelerenService(mockHttpClient.Object, mockApiMessageHandler.Object);
            string result = await _datafordelerenService.GetJordstykkeFromDAR(addressId);
            result.Should().BeEquivalentTo(expectedJordstykkeValue);
        }

        [Theory]
        [MemberData(nameof(GetJordstykkeFromDarTestData))]
        public async Task DataFordelerenService_GetGrundIdFromBBR_ShouldReturnExpectedId(string jordstykke, string expectedGrundIdValue)
        {
            var mockHttpClient = new Mock<HttpClient>();
            var mockApiMessageHandler = new Mock<IApiMessageHandler>();
            mockApiMessageHandler.Setup(m => m.GetPropertyFromJson(It.IsAny<string>(), It.IsAny<string>())).Returns(expectedGrundIdValue);
            _datafordelerenService = new DatafordelerenService(mockHttpClient.Object, mockApiMessageHandler.Object);
            string result = await _datafordelerenService.GetGrundIdFromBBR(jordstykke);
            result.Should().BeEquivalentTo(expectedGrundIdValue);
        }

        public static IEnumerable<Object[]> GetJordstykkeFromDarTestData()
        {
            return new List<object[]>
            {
                new object[]
                {
                    "kjkjkjjdlskelsrklkfslefsklfmsem",
                    "457368"
                },
                new object[]
                {
                    "osejrs-sfnskf-sfnf94-fe9fek",
                    "827649"
                }
            };
        }
    }
}
