using EmergencyResponse.ExternalServices;
using EmergencyResponse.Model;
using EmergencyResponse.Services;
using EmergencyResponse.SharedClasses.Validation;
using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyResponseTest.IntegrationTest
{
    public class DataforsyningenServiceTests
    {
        private readonly Mock<HttpMessageHandler> _mockHandler;
        private readonly HttpClient _httpClient;
        private readonly DataforsyningenService _service;

        public DataforsyningenServiceTests()
        {
            _mockHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHandler.Object);
            _service = new DataforsyningenService(_httpClient, new ApiMessageHandler(), new AddressValidator());
        }

        public Address getAddress()
        {
            Address address = new Address("Rødegårdsvej", "98C", null, null, "5000", "Odense C");
            address.SetLatitude(55.39199473);
            address.SetLongitude(10.40834309);
            return address;
        }

        //Address list for expectedAddress output from DataForsyningenCircleResponse.json
        public List<Address> GetAddresses()
        {
            var addresses = new List<Address>();

            addresses.Add(new Address("Rødegårdsvej", "98C", null, null, "5000", "Odense C"));
            addresses.Add(new Address("Rødegårdsvej", "98C", "3", "th", "5000", "Odense C"));
            addresses.Add(new Address("Rødegårdsvej", "98C", "2", "th", "5000", "Odense C"));

            addresses[0].SetLatitude(55.39199473);
            addresses[0].SetLongitude(10.40834309);
            addresses[1].SetLatitude(55.39199473);
            addresses[1].SetLongitude(10.40834309);
            addresses[2].SetLatitude(55.39199473);
            addresses[2].SetLongitude(10.40834309);

            return addresses;
        }

        public void mockHandlerSetup(string jsonResponse, HttpStatusCode code)
        {
             _mockHandler.Protected()
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
        }

        [Fact]
        public async Task GetAddressesInCircleAsync_ReturnsCorrectData()
        {
            // Arrange
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JsonData", "DataForsyningenCircleResponse.json");
            string jsonResponse = File.ReadAllText(jsonFilePath);

            // Setup response from mock HttpMessageHandler
            mockHandlerSetup(jsonResponse, HttpStatusCode.OK);

            Address centerAddress = getAddress();
            List<Address> expectedAddresses = GetAddresses();

            // Act
            var result = await _service.GetAddressesInCircleAsync(centerAddress, 10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAddresses.Count, result.Count); // Ensure counts match
            
            // Checks that all expected addresses are correctly mapped and present in the result
            foreach (var expected in expectedAddresses)
            {
                var matchingAddress = result.FirstOrDefault(a =>
                    a.StreetName == expected.StreetName &&
                    a.HouseNumber == expected.HouseNumber &&
                    a.Floor == expected.Floor &&
                    a.Door == expected.Door &&
                    a.PostalCode == expected.PostalCode &&
                    a.PostalCodeName == expected.PostalCodeName
                );

                // Ensure each expected address has a match
                Assert.NotNull(matchingAddress); 
            }
        }
    }
}
