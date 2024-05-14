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
        public AddressesInBuildingTest()
        {

        }

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

        //[Theory]
        //[ClassData(typeof(GetAddressesInBuildingTestData))]
        //public void DatafordelerenService_GetAddressesInBuilding_ShouldReturnListOfAddresses(AddressDTO address, List<AddressDTO> expectedAddresses, List<string> responses)
        //{
        //    var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        //    mockHttpMessageHandler.Protected()
        //        .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("services.datafordeler.dk/DAR/DAR/2.0.0/rest/adresse")),
        //                                                       ItExpr.IsAny<System.Threading.CancellationToken>())
        //        .ReturnsAsync(new HttpResponseMessage
        //        {
        //            StatusCode = System.Net.HttpStatusCode.OK,
        //            Content = new StringContent(responses[0]),
        //        });

        //    mockHttpMessageHandler.Protected()
        //        .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("services.datafordeler.dk/BBR/BBRPublic/1/rest/grund")),
        //                                                       ItExpr.IsAny<System.Threading.CancellationToken>())
        //        .ReturnsAsync(new HttpResponseMessage
        //        {
        //            StatusCode = System.Net.HttpStatusCode.OK,
        //            Content = new StringContent(responses[1]),
        //        });

        //    mockHttpMessageHandler.Protected()
        //        .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("services.datafordeler.dk/BBR/BBRPublic/1/rest/enhed")),
        //                                                       ItExpr.IsAny<System.Threading.CancellationToken>())
        //        .ReturnsAsync(new HttpResponseMessage
        //        {
        //            StatusCode = System.Net.HttpStatusCode.OK,
        //            Content = new StringContent(responses[2]),
        //        });

        //    mockHttpMessageHandler.Protected()
        //        .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("services.datafordeler.dk/BBR/BBRPublic/1/rest/grund")),
        //                                                       ItExpr.IsAny<System.Threading.CancellationToken>())
        //        .ReturnsAsync(new HttpResponseMessage
        //        {
        //            StatusCode = System.Net.HttpStatusCode.OK,
        //            Content = new StringContent(responses[3]),
        //        });

        //    var httpClient = new HttpClient(mockHttpMessageHandler.Object);
        //    _datafordelerenService = new DatafordelerenService(httpClient);
        //    var result = _datafordelerenService.GetAddressesInBuilding(address);
        //    result.Should().BeEquivalentTo(expectedAddresses);
        //}

        //public class GetAddressesInBuildingTestData : IEnumerable<object[]>
        //{
        //    private readonly List<object[]> _data = new List<object[]>
        //    {
        //        //new object[]
        //        //{
        //        //    new AddressDTO("Æblevej", "15", "1", "b", "5000", "odense C"),
        //        //    new List<AddressDTO> {
        //        //        new AddressDTO("Æblevej", "15", "1", "a", "5000", "odense C", "nnkdnvkdn"),
        //        //        new AddressDTO("Æblevej", "15", "1", "c", "5000", "odense C", "joidsdlksjls"),
        //        //        new AddressDTO("Æblevej", "15", "2", "a", "5000", "odense C", "eksknfkskf"),
        //        //        new AddressDTO("Æblevej", "15", "2", "b", "5000", "odense C", "skjekskjf"),
        //        //        new AddressDTO("Æblevej", "15", "2", "c", "5000", "odense C", "ldgjrigdl") },
        //        //    new List<string> {
        //        //    "[{\"husnummer\": {\"jordstykke\":\"445327\"}}]",
        //        //    "[{\"id_lokalId\": \"95b196eb-40fd-4f04-8465-76be5f499200\"}]",
        //        //    "[{\"id_lokalId\": \"0252795a-cb08-4bac-9b13-edb37a7504be\",\"opgangList\":[{\"opgang:\":{\"adgangFraHusnummer\"}}]}]",
        //        //    "[{\"id_lokalId\": \"4884f1ae-ec6d-4004-a24b-b70a18439072\"},{\"id_lokalId\": \"586410d8-fcae-4930-b45e-46d820362ebf\"}]",

        //        //}


        //    };

        //    public IEnumerator<object[]> GetEnumerator() => _data.GetEnumerator();

        //    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        //}
    }
}
