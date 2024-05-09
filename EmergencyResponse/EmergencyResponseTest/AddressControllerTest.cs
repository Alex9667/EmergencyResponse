using EmergencyResponse.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmergencyResponse.Controller;
using Moq;
using EmergencyResponse.Interfaces;
using Xunit;
using System.Net;
using System.Runtime.InteropServices;
using Moq.Protected;
using System.Text.Json;
using System.Net.Http;
using EmergencyResponse.DTO;

namespace EmergencyResponseTest
{
    public class AddressControllerTest
    {
        private AddressController _addressController;
        public AddressControllerTest()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var httpClient = new HttpClient(mockHttpMessageHandler.Object);

            _addressController = new AddressController(httpClient);
        }

        [Theory]
        [InlineData("{\"StreetName\":\"æblevej\", \"HouseNumber\":\"15\", \"Floor\":1, \"Door\":\"b\", \"PostalCode\":\"5000\", \"PostalCodeName\": \"odense C\", \"AddressId\": \"khskhkshfdjflacoope\"}", "æblevej", "15", "1", "b", "5000", "odense C", "khskhkshfdjflacoope")]
        public void AddressController_CreateAddress_FromString_ShouldReturnCreatedAddress(string json, string streetName, string houseNumber, string? floor, string? door, string postalCode, string postalCodeName, string addressId)
        {
            var result = new Address(streetName, houseNumber, floor, door, postalCode, postalCodeName, addressId);
            var address = _addressController.CreateAddress(json);
            address.Should().BeEquivalentTo(result);
        }

        [Theory]
        [InlineData("æblevej", "15", "1", "b", "5000", "odense C", "khskhkshfdjflacoope")]
        public void AddressController_CreateAddress_FromValues_ShouldReturnCreatedAddress(string streetName, string houseNumber, string? floor, string? door, string postalCode, string postalCodeName, string addressId)
        {
            var result = new Address(streetName, houseNumber, floor, door, postalCode, postalCodeName, addressId);

            var address = _addressController.CreateAddress(streetName, houseNumber, floor, door, postalCode, postalCodeName, addressId);
            address.Should().BeEquivalentTo(result);
        }

        [Theory]
        [InlineData("")]
        public void AddressController_CreateAddressFromEmptyString_ShouldReturnNull(string json)
        {
            var result = _addressController.CreateAddress(json);
            result.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(GetAddressesForSurroundingAddresses))]
        public void AddressController_GetAddressesInBuilding_ShouldReturnListOfAddresses(Address address, List<Address> addressesInBuilding)
        {
            //Mock httpClient?
            var result = _addressController.GetAddressesInBuilding(address);
            result.Should().BeEquivalentTo(addressesInBuilding);
        }

        [Theory]
        [MemberData(nameof(GetAddressesForAddressSearch))]
        public async void AddressController_SearchAddress_ShouldReturnListOfPossibleAddresses(AddressDTO address, List<Address> returnedAddresses)
        {
            var expectedResponse = JsonSerializer.Serialize(returnedAddresses);
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<System.Threading.CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(expectedResponse),
                });

            var httpClient = new HttpClient(mockHttpMessageHandler.Object);
            _addressController = new AddressController(httpClient);
            var result = await _addressController.SearchAddress(address);
            result.Should().BeEquivalentTo(returnedAddresses);
        }

        public static IEnumerable<Object[]> GetAddressesForSurroundingAddresses()
        {
            return new List<object[]>
            {
                new object[] {
                    new Address("Æblevej", "15", "1", "b", "5000", "odense C", "hkdhkfjskf"),
                    new List<Address> {
                        new Address("Æblevej", "15", "1", "a", "5000", "odense C", "nnkdnvkdn"),
                        new Address("Æblevej", "15", "1", "c", "5000", "odense C", "joidsdlksjls"),
                        new Address("Æblevej", "15", "2", "a", "5000", "odense C", "eksknfkskf"),
                        new Address("Æblevej", "15", "2", "b", "5000", "odense C", "skjekskjf"),
                        new Address("Æblevej", "15", "2", "c", "5000", "odense C", "ldgjrigdl")
                    },
                    new object[] {
                        new Address("Hovedgaden", "7", "2", "th", "5230", "odense M", "hkdhkfjskf"),
                        new List<Address> {
                            new Address("Hovedgaden", "7", "1", "tv", "5230", "odense M", "hkdhkfjskf"),
                            new Address("Hovedgaden", "7", "2", "tv", "5230", "odense M", "hkdhkfjskf"),
                            new Address("Hovedgaden", "7", "3", "th", "5230", "odense M", "hkdhkfjskf"),
                            new Address("Hovedgaden", "7", "3", "tv", "5230", "odense M", "hkdhkfjskf"),
                            new Address("Hovedgaden", "7", "4", "th", "5230", "odense M", "hkdhkfjskf"),
                            new Address("Hovedgaden", "7", "4", "tv", "5230", "odense M", "hkdhkfjskf"),
                            new Address("Hovedgaden", "7", "1", "th", "5230", "odense M", "hkdhkfjskf")
                        }
                    },

                }
            };
        }

        //public AddressControllerTest()
        //{
        //    HttpClient httpClient = new HttpClient(); 
        //    _addressController = new AddressController(httpClient);
        //}

        public static IEnumerable<Object[]> GetAddressesForAddressSearch()
        {
            return new List<object[]> {
            new object[] {
                new Address("Æblevej", "15", "1", "b", "5000", "odense C"),
                new List<Address> {
                    new Address("Æblevej", "15", "1", "a", "5000", "odense C", "nnkdnvkdn"),
                    new Address("Æblevej", "15", "1", "c", "5000", "odense C", "joidsdlksjls"),
                    new Address("Æblevej", "15", "2", "a", "5000", "odense C", "eksknfkskf"),
                    new Address("Æblevej", "15", "2", "b", "5000", "odense C", "skjekskjf"),
                    new Address("Æblevej", "15", "2", "c", "5000", "odense C", "ldgjrigdl")
                }
            },
            new object[] {
                new Address("Hovedgaden", "7", null, null, "5230", "odense M"),
                new List<Address> {
                    new Address("Hovedgaden", "7", "1", "th", "5230", "odense M", "hkdhkfjskf"),
                    new Address("Hovedgaden", "7", "1", "tv", "5230", "odense M", "hkdhkfjskf"),
                    new Address("Hovedgaden", "7", "2", "tv", "5230", "odense M", "hkdhkfjskf"),
                    new Address("Hovedgaden", "7", "3", "th", "5230", "odense M", "hkdhkfjskf"),
                    new Address("Hovedgaden", "7", "3", "tv", "5230", "odense M", "hkdhkfjskf"),
                    new Address("Hovedgaden", "7", "4", "th", "5230", "odense M", "hkdhkfjskf"),
                    new Address("Hovedgaden", "7", "4", "tv", "5230", "odense M", "hkdhkfjskf")
                }
            },
            new object[] {
                new Address("Mellemvej", "7", null, null, "5230", "odense M"),
                new List<Address> {
                    new Address("Mellemvej", "7", null, null, "5230", "odense M", "hkdhkfjskf"),
                }
            },
        };

            //Address address = _addressController.GetAddress();
            //address.Should().NotBeNull();   
        }

        [Theory]
        [InlineData("Nygade 74 7430 Ikast")]
        public void AddressController_GetAddressBFE_ShouldReturnString(string address)
        {
            Task<int> bfe = _addressController.GetAddressBFE(address);
            bfe.ToString().Should().NotBeNullOrEmpty();
        }
    }
}
