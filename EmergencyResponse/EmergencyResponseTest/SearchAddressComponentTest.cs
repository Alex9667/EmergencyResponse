using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit;
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
    }
}
