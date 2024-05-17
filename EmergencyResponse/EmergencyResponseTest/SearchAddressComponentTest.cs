using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Bunit;
using EmergencyResponse.DTO;
using EmergencyResponse.ExternalServices.Interfaces;
using EmergencyResponse.Pages;
using EmergencyResponse.ExternalServices.Interfaces;
using Moq;
using EmergencyResponse.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace EmergencyResponseTest
{
    public class SearchAddressComponentTest
    {
        private readonly TestContext _ctx;
        private readonly Mock<IDataforsyningService> _mockDataforsyningService;
        private readonly Mock<IDatafordelerenService> _mockDatafordelerenService;

        public SearchAddressComponentTest()
        {
            _ctx = new TestContext();

            // Create mock services
            _mockDataforsyningService = new Mock<IDataforsyningService>();
            _mockDatafordelerenService = new Mock<IDatafordelerenService>();

            // Register the mock services in the test context
            _ctx.Services.AddSingleton<IDataforsyningService>(_mockDataforsyningService.Object);
            _ctx.Services.AddSingleton<IDatafordelerenService>(_mockDatafordelerenService.Object);
        }



        [Fact]
        public async void SearchAddress_ShouldReturnListOfAddresses()
        {
            // Setup the mock to return a list of addresses
            var addresses = new List<AddressDTO>
            {
                new AddressDTO
                {
                    StreetName = "Fjellerupvej",
                    HouseNumber = "7",
                    PostalCode = "5463",
                    PostalCodeName = "SomeCity",
                    Floor = "1",
                    Door = "A",
                    BFE = 12345
                }
            };
            _mockDataforsyningService.Setup(service => service.SearchAddress(It.IsAny<AddressDTO>())).ReturnsAsync(addresses);

            var cut = _ctx.RenderComponent<SearchAddressComponent>();

            // Act - simulate user input and button click
            var input1 = cut.Find("input[id='street']");
            input1.SetAttribute("value", "Fjellerupvej"); // Set first value
            input1.Change("Fjellerupvej"); // Simulate input change

            var input2 = cut.Find("input[id='houseNumber']");
            input2.SetAttribute("value", "7"); // Set second value
            input2.Change("7"); // Simulate input change

            var input3 = cut.Find("input[id='postalCode']");
            input3.SetAttribute("value", "5463"); // Set third value
            input3.Change("5463"); // Simulate input change

            var submitButton = cut.Find("button[id='submit']");
            submitButton.Click(); // Simulate button click

            // Assert
            var componentInstance = cut.Instance;
            var possibleAddresses = componentInstance.possibleAddresses;

            possibleAddresses.Should().NotBeEmpty(); // Assert the final result
        }

        [Theory]
        [MemberData(nameof(GetAddresses))]
        public void SelectAddressFromList_ShouldSetSelectedAddress(List<AddressDTO> addresses)
        {
            using var ctx = new TestContext();

            var mockDataFordelerService = new Mock<IDatafordelerenService>();
            var mockDataForsyningService = new Mock<IDataforsyningService>();

            ctx.Services.AddSingleton<IDatafordelerenService>(mockDataFordelerService.Object);
            ctx.Services.AddSingleton<IDataforsyningService>(mockDataForsyningService.Object);

            var component = ctx.RenderComponent<SearchAddressComponent>();

            component.Instance.possibleAddresses.AddRange(addresses);
            component.Render();
            var address = addresses[2];

            var row = component.Find($"tr:has(td:nth-child(2):contains('{address.StreetName}'))" +
                                    $":has(td:nth-child(3):contains('{address.HouseNumber}'))" +
                                    $":has(td:nth-child(4):contains('{address.Floor}'))" +
                                    $":has(td:nth-child(5):contains('{address.Door}'))" +
                                    $":has(td:nth-child(6):contains('{address.PostalCode}'))" +
                                    $":has(td:nth-child(7):contains('{address.PostalCodeName}'))");

            row.Click();
            var selectedAddress = component.Instance.selectedAddress;
            selectedAddress.Should().Be(addresses[2]);
        }

        public static IEnumerable<Object[]> GetAddresses()
        {
            return new List<object[]> {
                new object[] {
                    new List<AddressDTO> {
                        new AddressDTO("Æblevej", "15", "1", "a", "5000", "odense C", "nnkdnvkdn"),
                        new AddressDTO("Æblevej", "15", "1", "c", "5000", "odense C", "joidsdlksjls"),
                        new AddressDTO("Æblevej", "15", "2", "a", "5000", "odense C", "eksknfkskf"),
                        new AddressDTO("Æblevej", "15", "2", "b", "5000", "odense C", "skjekskjf"),
                        new AddressDTO("Æblevej", "15", "2", "c", "5000", "odense C", "ldgjrigdl")
                    },
                }
            };
        }
    }
}
