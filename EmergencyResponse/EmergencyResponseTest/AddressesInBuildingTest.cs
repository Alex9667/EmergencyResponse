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
using static System.Net.WebRequestMethods;

namespace EmergencyResponseTest
{
    public class AddressesInBuildingTest
    {
        IDatafordelerenService _datafordelerenService;

        [Theory]
        [MemberData(nameof(GetJordstykkeFromDarTestData))]
        public async Task DataFordelerenService_GetJordstykkeFromDAR_ShouldReturnExpectedNumber(string addressId, string expectedJordstykkeValue)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(string.Empty),
                });

            var mockHttpClient = new Mock<HttpClient>(mockHttpMessageHandler.Object);
            mockHttpClient.Object.BaseAddress = new Uri("https://services.datafordeler.dk/");

            var mockApiMessageHandler = new Mock<IApiMessageHandler>();
            mockApiMessageHandler.Setup(m => m.GetNestedPropertyFromJson(It.IsAny<string>(), It.IsAny<string[]>())).Returns(expectedJordstykkeValue);
            _datafordelerenService = new DatafordelerenService(mockHttpClient.Object, mockApiMessageHandler.Object);
            string result = await _datafordelerenService.GetJordstykkeFromDAR(addressId);
            result.Should().BeEquivalentTo(expectedJordstykkeValue);
        }

        [Theory]
        [MemberData(nameof(GetJordstykkeFromDarTestData))]
        public async Task DataFordelerenService_GetGrundIdFromBBR_ShouldReturnExpectedId(string jordstykke, string expectedGrundIdValue)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(string.Empty),
                });

            var mockHttpClient = new Mock<HttpClient>(mockHttpMessageHandler.Object);
            mockHttpClient.Object.BaseAddress = new Uri("https://services.datafordeler.dk/");
            var mockApiMessageHandler = new Mock<IApiMessageHandler>();
            mockApiMessageHandler.Setup(m => m.GetPropertyFromJson(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<string>() { expectedGrundIdValue });
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
