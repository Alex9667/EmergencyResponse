using EmergencyResponse.ExternalServices;
using EmergencyResponse.Services;
using EmergencyResponse.SharedClasses.Validation.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using EmergencyResponse.Model;
using Moq.Protected;

namespace EmergencyResponseTest
{
    public class DataforsyningenServiceTests
    {
        private readonly DataforsyningenService _service;
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly Mock<IApiMessageHandler> _apiMessageHandlerMock;
        private readonly Mock<IAddressValidator> _addressValidatorMock;
        private readonly HttpClient _httpClient;

        public DataforsyningenServiceTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _apiMessageHandlerMock = new Mock<IApiMessageHandler>();
            _addressValidatorMock = new Mock<IAddressValidator>();
            _service = new DataforsyningenService(_httpClient, _apiMessageHandlerMock.Object, _addressValidatorMock.Object);
        }

        [Fact]
        public async Task GetAddressesInCircleAsync_Success_ReturnsAddresses()
        {
            var fakeAddress = new Address("Elm Street", "123", "2", "1A", "1234", "Springfield");
            fakeAddress.SetLatitude(55.6761);
            fakeAddress.SetLongitude(12.5683);
            var jsonResponse = @"[
                {
                    ""vejnavn"": ""Elm Street"",
                    ""husnr"": ""123"",
                    ""etage"": ""2"",
                    ""dør"": ""1A"",
                    ""postnr"": ""1234"",
                    ""postnrnavn"": ""Springfield""
                }
            ]";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            _addressValidatorMock.Setup(v => v.Validate(It.IsAny<Address>()));

            var result = await _service.GetAddressesInCircleAsync(fakeAddress, 100);

            Assert.Single(result);
            _addressValidatorMock.Verify(v => v.Validate(It.IsAny<Address>()), Times.Once);
        }

        [Fact]
        public async Task GetAddressesInCircleAsync_ApiError_ThrowsHttpRequestException()
        {
            var fakeAddress = new Address("Elm Street", "123", null, null, "1234", "Springfield");
            fakeAddress.SetLatitude(55.6761);
            fakeAddress.SetLongitude(12.5683);

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("")
                });


            await Assert.ThrowsAsync<HttpRequestException>(() => _service.GetAddressesInCircleAsync(fakeAddress, 100));
        }

        [Fact]
        public async Task GetAddressesInCircleAsync_InvalidAddressInput_ThrowsArgumentException()
        {
            // Adress no valid latitude and longitude
            var invalidAddress = new Address("Elm Street", "123", null, null, "1234", "Springfield");
            // Latitude and Longitude are not set

            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _service.GetAddressesInCircleAsync(invalidAddress, 100));

            Assert.Contains("Center address must have valid latitude and longitude.", exception.Message);
        }



    }
}
