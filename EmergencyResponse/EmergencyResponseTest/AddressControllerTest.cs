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
        [InlineData("{\"Id\":\"1\", \"StreetName\":\"æblevej\", \"HouseNumber\":\"15\", \"Floor\":1, \"Door\":\"b\", \"PostalCode\":\"5000\", \"PostalCodeName\": \"odense C\", \"AddressId\": \"khskhkshfdjflacoope\"}", "1", "æblevej", "15", 1, "b", "5000", "odense C", "khskhkshfdjflacoope")]
        public void AddressController_CreateAddress_FromString_ShouldReturnCreatedAddress(string json, string id, string streetName, string houseNumber, int? floor, string? door, string postalCode, string postalCodeName, string addressId)
        {
            var result = new Address
            {
                Id = id,
                StreetName = streetName,
                HouseNumber = houseNumber,
                Floor = floor,
                Door = door,
                PostalCode = postalCode,
                PostalCodeName = postalCodeName,
                AddressId = addressId
            };
            var address = _addressController.CreateAddress(json);
            address.Should().BeEquivalentTo(result);
        }

        [Theory]
        [InlineData("1", "æblevej", "15", 1, "b", "5000", "odense C", "khskhkshfdjflacoope")]
        public void AddressController_CreateAddress_FromValues_ShouldReturnCreatedAddress(string id, string streetName, string houseNumber, int? floor, string? door, string postalCode, string postalCodeName, string addressId)
        {
            var result = new Address
            {
                Id = id,
                StreetName = streetName,
                HouseNumber = houseNumber,
                Floor = floor,
                Door = door,
                PostalCode = postalCode,
                PostalCodeName = postalCodeName,
                AddressId = addressId
            };
            var address = _addressController.CreateAddress(id, streetName, houseNumber, floor, door, postalCode, postalCodeName, addressId);
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
        public async void AddressController_SearchAddress_ShouldReturnListOfPossibleAddresses(Address address, List<Address> returnedAddresses)
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
                    new Address {Id="1", StreetName="Æblevej", HouseNumber="15", Floor=1, Door="b", PostalCode="5000", PostalCodeName="odense C", AddressId="hkdhkfjskf"},
                    new List<Address> {
                        new Address{Id="1", StreetName="Æblevej", HouseNumber="15", Floor=1, Door="a", PostalCode="5000", PostalCodeName="odense C", AddressId="nnkdnvkdn" },
                        new Address{Id="1", StreetName="Æblevej", HouseNumber="15", Floor=1, Door="c", PostalCode="5000", PostalCodeName="odense C", AddressId="joidsdlksjls" },
                        new Address{Id="1", StreetName="Æblevej", HouseNumber="15", Floor=2, Door="a", PostalCode="5000", PostalCodeName="odense C", AddressId="eksknfkskf" },
                        new Address{Id="1", StreetName="Æblevej", HouseNumber="15", Floor=2, Door="b", PostalCode="5000", PostalCodeName="odense C", AddressId="skjekskjf" },
                        new Address{Id="1", StreetName="Æblevej", HouseNumber="15", Floor=2, Door="c", PostalCode="5000", PostalCodeName="odense C", AddressId="ldgjrigdl" }
                    }
                },
                new object[] {
                    new Address {Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=2, Door="th", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf"},
                    new List<Address> {
                        new Address{Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=1, Door="th", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" },
                        new Address{Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=1, Door="tv", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" },
                        new Address{Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=2, Door="tv", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" },
                        new Address{Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=3, Door="th", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" },
                        new Address{Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=3, Door="tv", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" },
                        new Address{Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=4, Door="th", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" },
                        new Address{Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=4, Door="tv", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" }
                    }
                },

            };
        }

        //public AddressControllerTest()
        //{
        //    HttpClient httpClient = new HttpClient(); 
        //    _addressController = new AddressController(httpClient);
        //}

        public static IEnumerable<Object[]> GetAddressesForAddressSearch()
        {
            return new List<object[]>
            {
                new object[] {
                    new Address {Id="1", StreetName="Æblevej", HouseNumber="15", Floor=1, Door="b", PostalCode="5000", PostalCodeName="odense C"},
                    new List<Address> {
                        new Address{Id="1", StreetName="Æblevej", HouseNumber="15", Floor=1, Door="a", PostalCode="5000", PostalCodeName="odense C", AddressId="nnkdnvkdn" },
                        new Address{Id="1", StreetName="Æblevej", HouseNumber="15", Floor=1, Door="c", PostalCode="5000", PostalCodeName="odense C", AddressId="joidsdlksjls" },
                        new Address{Id="1", StreetName="Æblevej", HouseNumber="15", Floor=2, Door="a", PostalCode="5000", PostalCodeName="odense C", AddressId="eksknfkskf" },
                        new Address{Id="1", StreetName="Æblevej", HouseNumber="15", Floor=2, Door="b", PostalCode="5000", PostalCodeName="odense C", AddressId="skjekskjf" },
                        new Address{Id="1", StreetName="Æblevej", HouseNumber="15", Floor=2, Door="c", PostalCode="5000", PostalCodeName="odense C", AddressId="ldgjrigdl" }
                    }
                },
                new object[] {
                    new Address {Id="1", StreetName="Hovedgaden", HouseNumber="7", PostalCode="5230", PostalCodeName="odense M"},
                    new List<Address> {
                        new Address{Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=1, Door="th", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" },
                        new Address{Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=1, Door="tv", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" },
                        new Address{Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=2, Door="tv", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" },
                        new Address{Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=3, Door="th", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" },
                        new Address{Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=3, Door="tv", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" },
                        new Address{Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=4, Door="th", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" },
                        new Address{Id="1", StreetName="Hovedgaden", HouseNumber="7", Floor=4, Door="tv", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" }
                    }
                },
                new object[] {
                    new Address {Id="1", StreetName="Mellemvej", HouseNumber="7", PostalCode="5230", PostalCodeName="odense M"},
                    new List<Address> {
                        new Address{Id="1", StreetName="Mellemvej", HouseNumber="7", PostalCode="5230", PostalCodeName="odense M", AddressId="hkdhkfjskf" },
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
