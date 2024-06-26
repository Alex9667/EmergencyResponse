﻿using EmergencyResponse.Controller;
using EmergencyResponse.DTO;
using Moq.Protected;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmergencyResponse.Interfaces;
using EmergencyResponse.ExternalServices;
using EmergencyResponse.Services;
using EmergencyResponse.Model;
using EmergencyResponse.SharedClasses.Validation.Interfaces;

namespace EmergencyResponseTest
{
    public class SearchAddressTest
    {
        private DataforsyningenService _dataforsyningenService;

        public SearchAddressTest()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            var mockApiMessageHandler = new Mock<IApiMessageHandler>(MockBehavior.Strict);
            var mockAddressValidator = new Mock<IAddressValidator>(MockBehavior.Strict);

            _dataforsyningenService = new DataforsyningenService(httpClient, mockApiMessageHandler.Object, mockAddressValidator.Object);

        }
        [Theory]
        [MemberData(nameof(GetAddressesForAddressSearch))]
        public async void DataForsyningService_SearchAddress_ShouldReturnListOfPossibleAddresses(AddressDTO address, List<AddressDTO> returnedAddresses)
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(string.Empty),
                });
            
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            httpClient.BaseAddress = new Uri("https://api.dataforsyningen.dk/");
            var mockApiMessageHandler = new Mock<IApiMessageHandler>();
            mockApiMessageHandler.Setup(m => m.ParseAddressesFromDataForsyning(It.IsAny<string>())).Returns(returnedAddresses);
            var mockAddressValidator = new Mock<IAddressValidator>();

            _dataforsyningenService = new DataforsyningenService(httpClient, mockApiMessageHandler.Object, mockAddressValidator.Object);
            var result = await _dataforsyningenService.SearchAddress(address);
            result.Should().BeEquivalentTo(returnedAddresses);
        }

        public static IEnumerable<Object[]> GetAddressesForAddressSearch()
        {
            return new List<object[]> {
                new object[] {
                    new AddressDTO("Æblevej", "15", "1", "b", "5000", "odense C"),
                    new List<AddressDTO> {
                        new AddressDTO("Æblevej", "15", "1", "a", "5000", "odense C", "nnkdnvkdn"),
                        new AddressDTO("Æblevej", "15", "1", "c", "5000", "odense C", "joidsdlksjls"),
                        new AddressDTO("Æblevej", "15", "2", "a", "5000", "odense C", "eksknfkskf"),
                        new AddressDTO("Æblevej", "15", "2", "b", "5000", "odense C", "skjekskjf"),
                        new AddressDTO("Æblevej", "15", "2", "c", "5000", "odense C", "ldgjrigdl")
                    },
                },
                new object[] {
                    new AddressDTO("Hovedgaden", "7", null, null, "5230", "odense M"),
                    new List<AddressDTO> {
                        new AddressDTO("Hovedgaden", "7", "1", "th", "5230", "odense M", "hkdhkfjskf"),
                        new AddressDTO("Hovedgaden", "7", "1", "tv", "5230", "odense M", "hkdhkfjskf"),
                        new AddressDTO("Hovedgaden", "7", "2", "tv", "5230", "odense M", "hkdhkfjskf"),
                        new AddressDTO("Hovedgaden", "7", "3", "th", "5230", "odense M", "hkdhkfjskf"),
                        new AddressDTO("Hovedgaden", "7", "3", "tv", "5230", "odense M", "hkdhkfjskf"),
                        new AddressDTO("Hovedgaden", "7", "4", "th", "5230", "odense M", "hkdhkfjskf"),
                        new AddressDTO("Hovedgaden", "7", "4", "tv", "5230", "odense M", "hkdhkfjskf")
                    },
                },
                new object[] {
                    new AddressDTO("Mellemvej", "7", null, null, "5230", "odense M"),
                    new List<AddressDTO> {
                        new AddressDTO("Mellemvej", "7", null, null, "5230", "odense M", "hkdhkfjskf"),
                    },
                },
            };
        }
    }
}
